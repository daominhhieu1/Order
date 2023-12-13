$(document).ready(function () {
    GetCart();
});

function GetCart() {
    $.ajax({
        url: '/Cart/GetProductInCart',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (result) {
            var groupedData = {};
            $.each(result, function (key, item) {
                const productName = item.ProductName;

                if (!groupedData[productName]) {
                    groupedData[productName] = [];
                }

                groupedData[productName].push(item);
            });

            var html = '';
            var totalMoney = 0;

            for (const productName in groupedData) {
                if (groupedData.hasOwnProperty(productName)) {
                    const productList = groupedData[productName];

                    // Hiển thị tên sản phẩm nhóm
                    html += `<tr class="tbody-item">
                            <td colspan="6" class="product-group-title">${productName}</td>
                        </tr>`;

                    // Hiển thị sản phẩm trong nhóm
                    $.each(productList, function (index, product) {
                        html += `<tr class="tbody-item">
                                <td class="product-remove">
                                    <a class="remove" href="javascript:void(0)">×</a>
                                </td>
                                <td class="product-thumbnail">
                                    <div class="thumb">
                                        <a href="single-product.html">
                                            <img src="~/assets/images/shop/cart1.webp" width="68" height="84" alt="Image-HasTech">
                                        </a>
                                    </div>
                                </td>
                                <td class="product-name">
                                    <a class="title" href="single-product.html">${product.ProductSkuName}</a>
                                </td>
                                <td class="product-price">
                                    <span class="price">${formatCurrency.format(product.Price)}</span>
                                </td>
                                <td class="product-quantity">
                                    <div class="pro-qty">
                                        <input type="text" class="quantity" title="Quantity" value="${product.QuantityCart}">
                                    </div>
                                </td>
                                <td class="product-subtotal">
                                    <span class="price">${formatCurrency.format(product.QuantityCart * product.Price)}</span>
                                </td>
                            </tr>`;

                        totalMoney += product.QuantityCart * product.Price;
                    });
                }
            }

            // Thêm tổng tiền
            $('#total_amount').text(formatCurrency.format(totalMoney));

            // Hiển thị kết quả vào phần tử có id là "ul_modal_cart"
            $('#tbody_cart').html(html);
        },
        error: function (err) {
            console.log(err);
        }
    });

}