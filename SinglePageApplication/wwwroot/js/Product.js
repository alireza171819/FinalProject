
window.onload = GetProducts();

async function GetProducts() {
    var requestOptions = {
        method: 'GET',
        headers: {
            "accept": "text/plain",
            "Content-Type": "application/json"
        },
        redirect: 'follow'
    };

    const response = await fetch('/product/GetAll', requestOptions)
        .catch(error => console.log('error', error));
    var products = await response.json();
    products = products.getByIdProductDtos;
    console.log(products);
    var itemsContainer = $("#items-container");
    $("#items-container").empty();
    for (let i = 0; i < products.length; i++) {
        var itemsTempelet = $("#items-template").html();
        itemsTempelet = itemsTempelet.replace(/#Name#/g, products[i].productName)
            .replace(/#Description#/g, products[i].productDescription)
            .replace(/#UnitPrice#/g, products[i].unitPrice);
        console.log(itemsTempelet);
        itemsContainer.append(itemsTempelet);
    }
    return products;
}

async function Create() {
    var requestOptions = {
        method: 'POST',
        headers: {
            "accept": "text/plain",
            "Content-Type": "application/json"
        },
        redirect: 'follow'
    };
    const response = await fetch('/product/create', requestOptions)
        .catch(error => console.log('error', error));
    var products = await response.json();
    products = products;
    
    return products;
}