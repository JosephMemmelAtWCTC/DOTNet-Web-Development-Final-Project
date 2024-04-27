document.addEventListener("DOMContentLoaded", function() {
    fetchProducts();
  });
  document.getElementById("CategoryId").addEventListener("change", (e) => {
    document.getElementById('product_rows').dataset['id'] = e.target.value;
    fetchProducts();
  });
  document.getElementById('Discontinued').addEventListener("change", (e) => {
    fetchProducts();
  });
  // delegated event listener
  document.getElementById('product_rows').addEventListener("click", (e) => {
    p = e.target.parentElement;
    if (p.classList.contains('product')) {
      e.preventDefault()
      // console.log(p.dataset['id']);
      if (document.getElementById('User').dataset['customer'].toLowerCase() == "true") {
        document.getElementById('ProductId').innerHTML = p.dataset['id'];
        document.getElementById('ProductName').innerHTML = p.dataset['name'];
        document.getElementById('UnitPrice').innerHTML = Number(p.dataset['price']).toFixed(2);
        display_total();
        const cart = new bootstrap.Modal('#cartModal', {}).show();
      } else {
        // alert("Only signed in customers can add items to the cart");
        toast("Access Denied", "You must be signed in as a customer to access the cart.");
      }
    }
  });
  const toast = (header, message) => {
    document.getElementById('toast_header').innerHTML = header;
    document.getElementById('toast_body').innerHTML = message;
    bootstrap.Toast.getOrCreateInstance(document.getElementById('liveToast')).show();
  }
  const display_total = () => {
    const total = parseInt(document.getElementById('Quantity').value) * Number(document.getElementById('UnitPrice').innerHTML);
    document.getElementById('Total').innerHTML = numberWithCommas(total.toFixed(2));
  }
  // update total when cart quantity is changed
  document.getElementById('Quantity').addEventListener("change", (e) => {
    display_total();
  });
  // function to display commas in number
  const numberWithCommas = x => x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
  async function fetchProducts() {
    const id = document.getElementById('product_rows').dataset['id'];
    const discontinued = document.getElementById('Discontinued').checked ? "" : "/discontinued/false";
    const { data: fetchedProducts } = await axios.get(`../../api/category/${id}/product${discontinued}`);
    // console.log(fetchedProducts);
    let product_rows = "";
    // console.log("fetchedProducts", fetchedProducts);
    fetchedProducts.map(product => {
      const css = product.discontinued ? " discontinued" : "";
      // product_rows += 
      //   `<tr class="product${css}" data-id="${product.productId}" data-name="${product.productName}" data-price="${product.unitPrice}">
      //     <td>${product.productName}</td>
      //     <td class="text-end">${product.unitPrice.toFixed(2)}</td>
      //     <td class="text-end">${product.unitsInStock}</td>
      //     <!--<td class="text-start">${product.rating}</td>-->
      //   </tr>`;
      //   // <td class="text-start">
      //   // Visuals only
      //   //   <i class="bi star bi-star-fill"></i>
      //   //   <i class="bi star bi-star-fill"></i>
      //   //   <i class="bi star bi-star-fill"></i>
      //   //   <i class="bi star bi-star"></i>
      //   //   <i class="bi star bi-star"></i>
      //   // </td>
      // console.log("EEE", product.productName);
      product_rows += 
        `
        <div class="accordion-item">
          <h2 class="accordion-header">
            <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#accordion_product_${product.productId}" aria-expanded="true" aria-controls="collapseOne">
            <table
              <tr class="product${css}" data-id="${product.productId}" data-name="${product.productName}" data-price="${product.unitPrice}">
                <td>${product.productName}</td>
                <td class="text-end">${product.unitPrice.toFixed(2)}</td>
                <td class="text-end">${product.unitsInStock}</td>
                <!--<td class="text-start">${product.rating}</td>-->
              </tr>
            </table>
            </button>
          </h2>
          <div id="accordion_product_${product.productId}" class="accordion-collapse collapse" >
            <div class="accordion-body">
              <strong>This is the first item's accordion body.</strong> It is shown by default, until the collapse plugin adds the appropriate classes that we use to style each element. These classes control the overall appearance, as well as the showing and hiding via CSS transitions. You can modify any of this with custom CSS or overriding our default variables. It's also worth noting that just about any HTML can go within the <code>.accordion-body</code>, though the transition does limit overflow.
            </div>
          </div>
        </div>
        `;

        // `
        // <tr class="product${css}" data-id="${product.productId}" data-name="${product.productName}" data-price="${product.unitPrice}">
        //   <td>${product.productName}</td>
        //   <td class="text-end">${product.unitPrice.toFixed(2)}</td>
        //   <td class="text-end">${product.unitsInStock}</td>
        //   <!--<td class="text-start">${product.rating}</td>-->
        // </tr>`;

        // <td class="text-start">
        // Visuals only
        //   <i class="bi star bi-star-fill"></i>
        //   <i class="bi star bi-star-fill"></i>
        //   <i class="bi star bi-star-fill"></i>
        //   <i class="bi star bi-star"></i>
        //   <i class="bi star bi-star"></i>
        // </td>
    });
    document.getElementById('product_rows').innerHTML = product_rows;
  }
  document.getElementById('addToCart').addEventListener("click", (e) => {
    // hide modal
    const cart = bootstrap.Modal.getInstance(document.getElementById('cartModal')).hide();
    // use axios post to add item to cart
    item = {
      "id": Number(document.getElementById('ProductId').innerHTML),
      "email": document.getElementById('User').dataset['email'],
      "qty": Number(document.getElementById('Quantity').value)
    }
    postCartItem(item);
  });
    async function postCartItem(item) {
    axios.post('../../api/addtocart', item).then(res => {
      toast("Product Added", `${res.data.product.productName} successfully added to cart.`);
    });
}