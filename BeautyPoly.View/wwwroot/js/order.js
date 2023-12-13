var productList = [];
var orderList = [];
$(document).ready(function () {
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
    if (count > 0) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Không thể lưu',
            showConfirmButton: false,
            timer: 1000
        })
        return false;
    }
    return true;
}

function createOrder() {
    if (!validate()) return;
    var OrderID = parseInt($('#orderid_order').val());
    var CustomerName = $('#customer_name').val();
    var Address = $('#customer_address').val(); 
    var Note = $('#order_note').val();

    var order = {
        OrderID: OrderID,
        CustomerName: CustomerName,
        Address: Address,
        Note: Note
    }
    var obj = {
        Order: order,
    }

    $.ajax({
        url: '/admin/order/create',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify(obj),
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

function addSanPham() {
    // Tạo HTML cho sản phẩm mới
    var htmlOption = '<option value="0" selected disabled>--Chọn sản phẩm--</option>';
    
    var html = `
     <tr>
        <td class="text-center"><a class="btn btn-sm btn-danger"><i class="bx bx-trash"></i></a></td>
        <td class="text-start">
            <select class="form-control">
            </select>
        </td>
        <td class="text-end">
            <input type="number" class="form-control"/>
        </td>
        <td class="text-end">
            <input type="number" class="form-control" />
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
    index++;
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

