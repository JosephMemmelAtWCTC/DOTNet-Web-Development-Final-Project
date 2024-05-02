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
  // const allAddToCarts = document.querySelectorAll('#product_rows button.add-to-cart');
  // allAddToCarts.forEach(addToCart => {
  //   addToCart.addEventListener("click", (e) => {
  //     console.log("p.dataset['id']----");

  //     p = e.target.parentElement.parentElement;
  //     if (p.classList.contains('product')) {
  //       console.log("p.dataset['id']");
  //       e.preventDefault()
  //       // console.log(p.dataset['id']);
        // if (document.getElementById('User').dataset['customer'].toLowerCase() == "true") {
        //   document.getElementById('ProductId').innerHTML = p.dataset['id'];
        //   document.getElementById('ProductName').innerHTML = p.dataset['name'];
        //   document.getElementById('UnitPrice').innerHTML = Number(p.dataset['price']).toFixed(2);
        //   display_total();
        //   const cart = new bootstrap.Modal('#cartModal', {}).show();
        // } else {
        //   // alert("Only signed in customers can add items to the cart");
        //   toast("Access Denied", "You must be signed in as a customer to access the cart.");
        // }
  //     }
  //   });
  // });
  function addToCartPullUpModal(productId, productName, unitPrice, unitsInStock, rating){
    console.log("addToCartPullUpModal");
     if (document.getElementById('User').dataset['customer'].toLowerCase() == "true") {
      document.getElementById('ProductId').innerHTML = productId;
      document.getElementById('ProductName').innerHTML = productName;
      document.getElementById('UnitPrice').innerHTML = Number(unitPrice).toFixed(2);
      display_total();
      const cart = new bootstrap.Modal('#cartModal', {}).show();
    } else {
      // alert("Only signed in customers can add items to the cart");
      toast("Access Denied", "You must be signed in as a customer to access the cart.");
    }
  }
  
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
    let product_row = "";
    // console.log("fetchedProducts", fetchedProducts);
    document.getElementById('product_rows').innerHTML = "";
    fetchedProducts.map(product => {
      const css = product.discontinued ? " discontinued" : "";

      // TODO: Add paganation and wait for everything else to load before loading reviews

      product_row = 
        `
        <tr>
          <td class="w-100">
            <div class="accordion-item" onclick="loadReviewsForProduct(${product.productId})">
              <h2 class="accordion-header">
                <table class="w-100">
                  <tr class="product${css} w-100 accordion-button collapsed"
                      data-id="${product.productId}"
                      data-name="${product.productName}"
                      data-price="${product.unitPrice}"
                      data-bs-toggle="collapse"
                      data-bs-target="#accordion_product_${product.productId}"
                      aria-expanded="true"
                      aria-controls="collapse_${product.productId}">
                    <div class="row row-cols-4">
                      <td class="test-start col-6">${product.productName}</td>
                      <td class="text-end col">${product.unitPrice.toFixed(2)}</td>
                      <td class="text-end col">${product.unitsInStock}</td>
                      <td class="text-end col">${product.rating}</td>
                      <td class="text-end col">
                        <button class="add-to-cart" onclick="addToCartPullUpModal('${product.productId}','${product.productName}','${product.unitPrice}','${product.unitsInStock}','${product.rating}')">
                          <i class="bi bi-cart-plus"></i>
                        </button>
                      </td>
                    </div>
                  </tr>
                </table>
              </h2>
              <div id="accordion_product_${product.productId}" class="accordion-collapse collapse" >
                <div class="accordion-body">
                  <h3>Reviews for ${product.productName}</h3>
                  <div class="reviews list-group">
                  </div>
                  <!--<strong>This is the first item's accordion body.</strong> It is shown by default, until the collapse plugin adds the appropriate classes that we use to style each element. These classes control the overall appearance, as well as the showing and hiding via CSS transitions. You can modify any of this with custom CSS or overriding our default variables. It's also worth noting that just about any HTML can go within the <code>.accordion-body</code>, though the transition does limit overflow.-->
                </div>
              </div>
            </div>
          </td>
        </tr>
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

        // Add to page right away as api calls are used in getting the reviews - don't delay
        document.getElementById('product_rows').innerHTML += product_row;
    });
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

  async function loadReviewsForProduct(productId){
    console.log("loadReviewsForProduct", productId);
    const reviewsArea = document.querySelector("#accordion_product_"+productId+" .reviews");
    if(reviewsArea.children.length === 0){
      console.log("Reviews not already loaded, retrieving reviews.");
      const { data: fetchedReviews } = await axios.get(`../../api/product/reviews/${productId}`);
      fetchedReviews.map(review => {
        console.log("Review", review);

        let ratingsDisplay = "";
        let starNum = 0;
        for(; starNum<review.rating; starNum++){
          ratingsDisplay += `<i class="bi bi-star-fill"></i>`;
        };
        for(; starNum<5; starNum++){
          ratingsDisplay += `<i class="bi bi-star"></i>`;
        };
        reviewsArea.innerHTML += `
          <div href="#" class="list-group-item list-group-item-action">
            <div class="d-flex w-100 justify-content-between">
              <h5 class="mb-1">${review.customer.companyName}</h5>
              <small class="text-body-secondary">${ratingsDisplay}</small>
            </div>
            <p class="mb-1 ms-3">${review.comment}.</p>
            <small class="text-body-secondary">${review.reviewAt}</small>
          </a>
        `;
      });

    }else{
      console.log("Reviews already exist");
    }
  }