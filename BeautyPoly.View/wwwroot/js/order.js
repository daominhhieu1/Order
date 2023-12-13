var productList = [];
var orderList = [];
$(document).ready(function () {
    $.ajax({
        url: '/admin/order/get-product',
        type: 'Get',
        success: function (result) {
            if (result.length > 0) {
                productList = result;
            }
            console.log(result);
        },
        error: function (err) {
            console.log(err)
        }
    });
    document.addEventListener('click', function (event) {
        if (event.target.classList.contains('bx-trash')) {
            var clickedTrashButton = event.target;
            var parentRow = clickedTrashButton.closest('.pick-prod');

            if (parentRow) {
                parentRow.remove(); // Remove the entire row
            }
        }
    });
    $("#home-tab").on("click", function () {
        $.ajax({
            url: '/admin/order/1',
            type: 'Get',
            success: function (result) {
                $("#tbody_order_hc").empty();
                var html = ``;
                var index = 0;
                result.forEach(function (element) {
                    console.log(element);
                    html += `
                    <tr>
                                            <th style="vertical-align: top;">${++index}</th>
                                            <th style="vertical-align: top;"><a href="#" onclick="addHDOrder()" class="card-link">${element.OrderCode}</a></th>
                                            <th style="vertical-align: top;">Admin</th>
                                            <th style="vertical-align: top;">${element.CustomerName}</th>
                                            <th style="vertical-align: top;">${element.CustomerPhone}</th>
                                            <th style="vertical-align: top;white-space: nowrap;">${element.Address}</th>
                                            <th style="vertical-align: top;"></th>
                                            <th style="vertical-align: top;"></th>
                                            <th style="vertical-align: top;">${element.TotalMoney}</th>
                                            <th style="vertical-align: top;">${element.MedthodPayment}</th>
                                            <th style="vertical-align: top;">${element.OrderDate}</th>
                                            <th style="vertical-align: top;">${element.ShipDate}</th>
                                            <th style="vertical-align: top;">${element.PaymentDate}</th>
                                            <th style="vertical-align: top;white-space: nowrap;">${element.Note}</th>
                                        </tr>
                    `;
                });
                $("#tbody_order_hc").append(html);
                console.log(result);
            },
            error: function (err) {
                console.log(err)
            }
        });
    });
    $("#profile-tab").on("click", function () {
        $.ajax({
            url: '/admin/order/2',
            type: 'Get',
            success: function (result) {
                $("#tbody_order_dd").empty();
                var html = ``;

                if (result.length > 0) {
                    var index = 0;
                    result.forEach(function (element) {
                        console.log(element);
                        html += `
                                    <tr>
                                            <th style="vertical-align: top;">${++index}</th>
                                            <th style="vertical-align: top;"><a href="#" onclick="addHDOrder()" class="card-link">${element.OrderCode}</a></th>
                                            <th style="vertical-align: top;">Admin</th>
                                            <th style="vertical-align: top;">${element.CustomerName}</th>
                                            <th style="vertical-align: top;">${element.CustomerPhone}</th>
                                            <th style="vertical-align: top;white-space: nowrap;">${element.Address}</th>
                                            <th style="vertical-align: top;"></th>
                                            <th style="vertical-align: top;"></th>
                                            <th style="vertical-align: top;">${element.TotalMoney}</th>
                                            <th style="vertical-align: top;">${element.MedthodPayment}</th>
                                            <th style="vertical-align: top;">${element.OrderDate}</th>
                                            <th style="vertical-align: top;">${element.ShipDate}</th>
                                            <th style="vertical-align: top;">${element.PaymentDate}</th>
                                            <th style="vertical-align: top;white-space: nowrap;">${element.Note}</th>
                                        </tr>
                    `;
                    });
                } else {
                    alert("Không tồn tại hóa đơn nào!");
                }
                $("#tbody_order_dd").append(html);
            },
            error: function (err) {
                console.log(err)
            }
        });
    });
});
function add() {
    $('#modal_order').modal('show');
}

function addHDOrder() {
    $('#showHD').modal('show');
}

function validate() {
    var count = 0;
    var orderCode = $('#order_code').val();
    if (orderCode == '') {
        count++;
    }
    //if (count > 0) {
    //    Swal.fire({
    //        icon: 'error',
    //        title: 'Oops...',
    //        text: 'Không thể lưu',
    //        showConfirmButton: false,
    //        timer: 1000
    //    })
    //    return false;
    //}
    return true;
}

function createOrder() {
    if (!validate()) return;
    var order = {
        OrderID: parseInt($('#orderid_order').val()),
        CustomerName: $('#customer_name').val(),
        Address: $('#customer_address').val(),
        Note: $('#order_note').val(),
        CustomerPhone: $('#customer_phone').val(),
        MedthodPayment: $("#payment_method").val(),
    }
    var prods = [];
    $(".pick-prod").each(function (index) {
        console.log($(this).find(".item-prod").val());
        console.log($(this).find(".item-quantity").val());
        console.log($(this).find(".item-price").val());
        prods.push({
            ProductID: $(this).find(".item-prod").val(),
            Quantity: $(this).find(".item-quantity").val(),
            Price: $(this).find(".item-price").val()
        });
    });
    order.prods = prods;
    console.log(prods);
    $.ajax({
        url: '/admin/order/create',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify(order),
        success: function (result) {
            if (result == 1) {
                GetAll();
                $('#modal_order').modal('hide');
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: `${result}`,
                    showConfirmButton: false,
                    timer: 1000
                })
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}
function remove(event) {
    //// Get the element that was clicked
    //var clickedElement = event.target;
    //console.log($(event).parent().parent());

    //// Perform actions on the clicked element
    //// For instance, you can remove the parent element of the clicked button
    ////$(clickedElement.parentNode.parentNode).parent().remove();
    //clickedElement.parentNode.parentNode.remove();
}
function addSanPham() {
    // Tạo HTML cho sản phẩm mới
    var htmlOption = '<option value="0" selected disabled>--Chọn sản phẩm--</option>';
    var select2Prod = "";
    if (productList.length > 0) {
        productList.forEach(function (element) {
            select2Prod += ` <option value="${element.ProductID}">${element.ProductName}</option>`;
        });
    }
    var html = `
     <tr class="pick-prod">
        <td class="text-center" onclick="remove(event)"><a class="btn btn-sm btn-danger trash-button" ><i class="bx bx-trash"></i></a></td>
        <td class="text-start">
            <select class="form-control item-prod">
            ${select2Prod}
            </select>
        </td>
        <td class="text-end ">
            <input type="number" class="form-control item-quantity"/>
        </td>
        <td class="text-end ">
            <input type="number" class="form-control item-price" />
        </td>

    </tr>`;

    // Thêm HTML sản phẩm mới vào tbody_product
    $('#tbody_product').append(html);

    // Thêm option vào select2
    $(`#abc`).html(htmlOption);
    $(`#abc`).select2({
        dropdownParent: $("#modal_order"),
        theme: "bootstrap-5",
    });

    // Tăng giá trị index
    // index++;
}

//function addOrder() {
//    // Tạo HTML cho sản phẩm mới
//    var htmlOption = '<option value="0" selected disabled>--Chọn sản phẩm--</option>';

//    var html = `
//     <th><button class="nav-link active" id="home-tabs" data-bs-toggle="tab" data-bs-target="#bordered-home" type="button" role="tab" aria-controls="home" aria-selected="true">Hoán đơn 1</button></th>`;

//    // Thêm HTML sản phẩm mới vào tbody_product
//    $('#tbody_add_order').append(html);

//    // Thêm option vào select2


//    // Tăng giá trị index
//    index++;
//}

