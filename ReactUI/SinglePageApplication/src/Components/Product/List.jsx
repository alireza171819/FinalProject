
function formatCurrency(value) {
            return `$${value.toFixed(2)}`;
}

export function List(){
    return (
          <div class="table-responsive">
                <table class="table align-middle mb-0" id="productsTable" onLoad={GetProducts()}>
                    <thead class="table-light">
                        <tr>
                            <th scope="col">Name</th>
                            <th scope="col">Category</th>
                            <th scope="col" class="text-end">Price</th>
                            <th scope="col" class="text-end">Stock</th>
                            <th scope="col" class="text-center">Actions</th>
                        </tr>
                    </thead>
                    <tbody id="products-container">
                        
                    </tbody>
                </table>
            </div>
    )
}

async function  GetProducts() {
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
    var itemsContainer = document.getElementById("products-container");
    itemsContainer.innerHTML= "";
    const fragment = document.createDocumentFragment();
    for (let i = 0; i < products.length; i++) {
        <tr>
            <td>
                <div class="fw-semibold">${products[i].productName}</div>
                <small class="text-muted">${products[i].productDescription}</small>
            </td>
            <td>${product.category}</td>
            <td class="text-end">${formatCurrency(products[i].unitPrice)}</td>
            <td class="text-center">
                <div class="btn-group btn-group-sm" role="group">
                    <button type="button" class="btn btn-outline-primary" data-action="edit" data-id="${product.id}"><i class="bi bi-pencil"></i> Edit</button>
                    <button type="button" class="btn btn-outline-danger" data-action="delete" data-id="${product.id}"><i class="bi bi-trash"></i> Delete</button>
                </div>
            </td>
        </tr>
        fragment.appendChild(row);
    }
    itemsContainer.appendChild(fragment);
}
