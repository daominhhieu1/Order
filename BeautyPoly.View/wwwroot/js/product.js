var arrCategory = [];
var arrOption = [];
var arrOptionValue = [];
var index = 1;
class DynamicArrayCombiner {
    constructor() {
        this.arrays = [];
        this.combinations = [];
    }

    addArray(array) {
        this.arrays.push(array);
        this.generateCombinations();
    }

    removeArray(array) {
        const index = this.arrays.indexOf(array);
        if (index !== -1) {
            this.arrays.splice(index, 1);
            this.generateCombinations();
        }
    }
    removeAllArrays() {
        this.arrays = [];
        this.generateCombinations();
    }
    generateCombinations() {
        const result = [];

        const combineArrays = (currentCombo, arrayIndex) => {
            if (arrayIndex === this.arrays.length) {
                result.push(currentCombo.join(" - "));
                return;
            }

            const currentArray = this.arrays[arrayIndex];
            for (const item of currentArray) {
                currentCombo.push(item);
                combineArrays(currentCombo, arrayIndex + 1);
                currentCombo.pop();
            }
        };

        combineArrays([], 0);
        this.combinations = result;
    }
}



const arrayCombiner = new DynamicArrayCombiner();
const arrayCombinerText = new DynamicArrayCombiner();

$(document).ready(function () {
    GetCategory();
    GetOption();
    GetAllOptionValue();
    $('#uploadBtn').click(function () {
        $('#fileInput').click();
    });

    $('#fileInput').change(function () {
        displayImages(this.files);
    });

})
function displayImages(files) {

    var previewContainer = $('#imagePreviewContainer');
    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        var reader = new FileReader();
        reader.onload = function (e) {
            var html = `<div class="col-4 ps-1 pe-1 mb-4 image-container" style="max-height:370px;position: relative;">
                            <a class="btn text-danger" onclick="removeImage(this)" style="cursor:pointer; position: absolute; top: 4px; right: 4px;"><i class="bi bi-trash"></i></a>
                            <img src="${e.target.result}" width="100%" height="100%" />
                        </div>`;
            previewContainer.append(html);
        };
        reader.readAsDataURL(file);
    }
}
function removeImage(button) {
    $(button).closest('.col-4').remove();
    $('#fileInput').val(null);

}
function add() {
    $('#product_code_product').val('');

    $('#modal_product').modal('show');
}


function GetCategory() {
    $.ajax({
        url: '/admin/category/getall',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (result) {
            arrCategory = result;
            var html = '<option selected disabled>--Chọn danh mục--</option>';
            $.each(result, function (key, item) {
                html += ` <option value="${item.cateId}">${item.cateName}</option>`
            });

            $('#cate_id_product').html(html);
        },
        error: function (err) {
            console.log(err)
        }
    });
}



function addOption() {
    var htmlOption = '<option value="0" selected disabled>--Chọn thuộc tính--</option>';
    $.each(arrOption, function (key, item) {
        htmlOption += `<option value="${item.OptionID}">${item.OptionName}</option>`
    });

    var html = `<div class="row mt-2">
                    <div class="col-12 col-md-4">
                        <select class="form-control" id="option_product_${index}" onchange="GetOptionValue(this.value,this.id)" >
                        </select>
                    </div>
                    <div class="col-12 col-md-4" id="div_option_value_${index}">
                    </div>
                      <div class="col-12 col-md-4">
                        <div class="d-flex justify-content-end">
                            <button class="btn text-danger" onclick="deleteRow(this)"><i class="bi bi-trash"></i></button>
                        </div>
                    </div>
                </div>`
    $('#option_value_product').append(html);
    $(`#option_product_${index}`).html(htmlOption);
    $(`#option_product_${index}`).select2({
        dropdownParent: $("#modal_product"),
        theme: "bootstrap-5",
    })
    index++;
}


function GetOptionValue(id, selectID) {
    var i = selectID.split('_')[2];
    $.ajax({
        url: '/admin/option/getallvalue',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        data: { optionID: id },
        success: function (result) {
            var html = ` <select class="form-control" id="option_value_product_${i}" state="states" multiple="multiple">
                </select> `
            $(`#div_option_value_${i}`).html(html);
            var htmlValue = ''
            $.each(result, function (key, item) {
                htmlValue += `<option value="${item.OptionValueID}" >${item.OptionValueName}</option>`
            })
            $(`#option_value_product_${i}`).html(htmlValue);
            $(`#option_value_product_${i}`).select2({
                dropdownParent: $("#modal_product"),
                placeholder: '---Chọn giá trị---',
                theme: "classic"
            });
            $(`#option_value_product_${i}`).on('change', function () {
                optionValueChange();
            });
        },
        error: function (err) {
            console.log(err)
        }
    });
}


function GetOption() {
    $.ajax({
        url: '/admin/product/get-option',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (result) {
            arrOption = result;
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function deleteRow(button) {
    // Find the parent row element and remove it
    const row = button.closest('.row');
    if (row) {
        row.remove();
        optionValueChange()
    }
}

function addOption1() {
    $('#modal_option').modal('show');
}

function GetAllOptionValue() {
    $.ajax({
        url: '/admin/product/get-option-value',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (result) {
            arrOptionValue = result;
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function optionValueChange() {
    arrayCombiner.removeAllArrays();
    arrayCombinerText.removeAllArrays();
    var allSelectedValues = [];
    var allSelectText = [];
    for (var j = 1; j <= index; j++) {
        var values = $(`#option_value_product_${j}`).val();

        if (values && values.length > 0) {
            var texts = arrOptionValue.filter(p => values.map(Number).includes(p.OptionValueID)).map(o => o.OptionValueName);

            allSelectedValues.push(values);
            allSelectText.push(texts);
        }
    }

    for (var j = 0; j < allSelectedValues.length; j++) {
        arrayCombiner.addArray(allSelectedValues[j]);
        arrayCombinerText.addArray(allSelectText[j]);
    }
    var productVariants = arrayCombinerText.combinations;
    var combinedArray = [];

    for (var i = 0; i < arrayCombinerText.combinations.length; i++) {
        var combinedObject = {
            value: arrayCombiner.combinations[i],
            text: arrayCombinerText.combinations[i]
        };
        combinedArray.push(combinedObject);
    }


    var html = '';


    var indexRow = 1;
    $.each(combinedArray, function (key, item) {
        html += `<tr id="row_option_value_product_${indexRow}">
                    <td>${item.text}</td>
                    <td><input type="number" id="capital_price_sku_${indexRow}" class="text-end form-control" value="0" /></td>
                    <td><input type="number" id="price_sku_${indexRow}" class="text-end form-control" value="0" /></td>
                    <td><input type="number" id="quantity_sku_${indexRow}" class="text-end form-control" value="0" /></td>
                    <td><input type="text" id="option_value_sku_${indexRow}" class="text-end form-control" value="${item.value}" hidden/></td>
                </tr>`
        indexRow++;
    });
    $("#tbody_product_detail").html(html);
}

function validate() {
    var productCode = $('#product_code_product');
    var productName = $('#product_name_product');
    var cateId = $('#cate_id_product');

    productCode.removeClass('is-invalid');
    productName.removeClass('is-invalid');
    cateId.removeClass('is-invalid');

    var productCodeValue = productCode.val().trim();
    var productNameValue = productName.val().trim();
    var cateIdValue = parseInt(cateId.val());

    if (productCodeValue === '') {

        productCode.addClass('is-invalid');
    }
    if (productNameValue === '') {

        productName.addClass('is-invalid');
    }
    if (isNaN(cateIdValue) || cateIdValue <= 0) {

        cateId.addClass('is-invalid');
    }
    if (productCodeValue === '' || productNameValue === '' || isNaN(cateIdValue) || cateIdValue <= 0) {
        return false;
    }
    var uniqueOptionIDs = [];

    $('select[id*="option_product_"]').each(function () {
        var optionID = parseInt($(this).val());

        if (uniqueOptionIDs.includes(optionID)) {
            $(this).addClass('is-invalid');
            return false;
        } else {
            uniqueOptionIDs.push(optionID);
            $(this).removeClass('is-invalid');
        }
    });

    return true;
}


function save() {
    if (!validate()) return;
    var productCode = $('#product_code_product').val();
    var productName = $('#product_name_product').val();
    var cateId = parseInt($('#cate_id_product').val());
    var listOptionID = [];
    var listSku = [];
    var objProduct = {
        ProductCode: productCode,
        ProductName: productName,
        CateID: cateId
    }
    $('select[id*="option_product_"]').each(function () {
        var optionID = parseInt($(this).val());
        listOptionID.push(optionID);
    });
    $('tr[id*="row_option_value_product_"]').each(function () {
        var id = $(this).attr('id');
        var numberId = id.substring(id.lastIndexOf('_') + 1);
        var capitalPrice = parseFloat($(`#capital_price_sku_${numberId}`).val());
        var price = parseFloat($(`#price_sku_${numberId}`).val());
        var quantity = parseInt($(`#quantity_sku_${numberId}`).val());
        var optionValueID = $(`#option_value_sku_${numberId}`).val();
        var objSku = {
            CapitalPrice: capitalPrice,
            Price: price,
            Quantity: quantity,
            OptionValueID: optionValueID
        }
        listSku.push(objSku);
    });

    var selectedImages = [];

    // Iterate through image containers
    $('.image-container').each(function () {

        var imageSource = $(this).find('img').attr('src');

        
        selectedImages.push(imageSource);

    });

    var obj = {
        Product: objProduct,
        ListOptionID: listOptionID,
        ListSku: listSku,
        Images: selectedImages
    }
    $.ajax({
        url: '/admin/product/create',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify(obj),
        success: function (result) {
            $('#modal_product').modal('hide');
        },
        error: function (err) {
            console.log(err)
        }
    });
}

