const ORDERS_URL = "/orders-hub";
const SEND_ORDER = "/api/v1/orders";
const ORDER_CREATED_NOTIFICATION = "order-created";
const ORDER_APPROVED_NOTIFICATION = "order-approved";
const ORDER_CANCELED_NOTIFICATION = "order-canceled";
const ORDER_PROCESSED_NOTIFICATION = "order-processed";
const ORDER_FAILED_NOTIFICATION = "order-failed";
const ORDER_ERROR_NOTIFICATION = "order-error";

const connection = new signalR.HubConnectionBuilder().withUrl(ORDERS_URL).build();
const orderList = document.getElementById("order-list");
const logError = error => console.error(error.toString());
const generatePrice = () => {
  return parseFloat((Math.random() * (1000.00 - 100.00 + 100.00) + 100.00).toFixed(2));
};
const currencyFormatter = new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
  maximumFractionDigits: 2
});

document.getElementById("send-order").addEventListener("click", event => {
  let customerName = 'OrderSimulator User';
  let itemsCount = parseInt(document.getElementById('items-count').value);
  let voucher = document.getElementById('voucher').value;
  let items = [...new Array(itemsCount)].map((_, index) => ({ name: `Product ${index + 1}`, amount: generatePrice() }));
  let orderCommand = { customerName, items, voucher };
  console.log(items);

  orderList.innerHTML = '';
  orderHandler.postOrder(orderCommand);
  writeUpdates("Sending the order. Waiting server response...");
});

const writeUpdates = html => {
  let li = document.createElement("li");
  li.innerHTML = `${html}<hr/>`;

  orderList.appendChild(li);
};

const orderHandler = {
  updateStatus(order) {
    writeUpdates(`Order <b>${order.id}</b> - <b>${order.status}</b>`);
  },
  displayError(errors) {
    let errorHtml = errors.reduce((html, error) => {
      return `${html}error: ${error.errorCode}, message: ${error.errorMessage}.</br>`
    }, `Server responded with errors:<br/>`);

    writeUpdates(errorHtml);
  },
  displayOrder(order) {
    let issuedDate = new Date(order.issuedAt);
    let title = `
      Order <b>${order.id}</b> - <b>${order.status}</b><br/>attending request <b>${order.requestId}</b>
      by <b>${ order.customer}</b> at <b>${issuedDate.toLocaleDateString("pt-BR")}.</b>`;

    let items = order.items.reduce((itemsHtml, item) => {
      return `${itemsHtml}<p>
        Item: <b>${item.name}</b><br/>
        Id: <b>${item.id}</b><br/>
        Value: <b>${currencyFormatter.format(item.amount)}</b><br/>
      </p>`;
    }, '');

    let totalAmount = `Order total amount: <b>${currencyFormatter.format(order.totalAmount)}</b>`;
    let voucher = order.voucher.code
      ? `Voucher applied: <b>${order.voucher.code}</b> - Discont: <b>${currencyFormatter.format(order.discount)}.</b>`
      : 'No Voucher applyied.';

    writeUpdates(`
      <div class="order-title">${title}</div>
      <hr/>
      <div class="order-body">
        <div class="order-paragraph">${items}</div>
        <hr/>
        <p class= "order-paragraph">${voucher}</p>
        <p class="order-paragraph">${totalAmount}</p>
      </div>
    `);
  },
  postOrder(order) {
    let request = new XMLHttpRequest();
    request.onreadystatechange = () => {
      if (request.readyState === 4) {
        let response = JSON.parse(request.response);
        if (request.status >= 400) {
          if (request.status === 400) writeUpdates(`Error on sending order. Status: ${request.status}.`);
          writeUpdates(`Server responded with ${response}.`)
        }
        else if (request.status == 202) {
          writeUpdates(`Server accepted the request <b>${response}</b>.`);
        }
      }
    };

    request.open("POST", SEND_ORDER);
    request.setRequestHeader("Content-Type", "application/json");
    request.send(JSON.stringify(order));
  }
};

connection.on(ORDER_CREATED_NOTIFICATION, orderHandler.updateStatus);
connection.on(ORDER_APPROVED_NOTIFICATION, orderHandler.updateStatus);
connection.on(ORDER_CANCELED_NOTIFICATION, orderHandler.updateStatus);
connection.on(ORDER_FAILED_NOTIFICATION, orderHandler.updateStatus);
connection.on(ORDER_PROCESSED_NOTIFICATION, orderHandler.displayOrder);
connection.on(ORDER_ERROR_NOTIFICATION, orderHandler.displayError);
connection.start(() => console.log("Orders client connected.")).catch(logError);
