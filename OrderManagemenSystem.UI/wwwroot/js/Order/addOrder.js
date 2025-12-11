function editableData(categories) {
    var orderFormData = [];

    var myTable = new DataTable('#myEditDataTable', {
        paging: false,
        searching: false,
        info: false
    })

    //add new row
    $("#btnAddNewOrderHistory").off("click").on('click', function () {
        var categoryOptions = "<option value=0>Select Category</option>";

        $.each(categories, function (index, item) {
            categoryOptions += "<option value=" + item.id + ">" + item.name + "</option>";
        });
        var ctgDrp = `<div class="form-group"><select class='ddlCategory form-select required'>
                            ${categoryOptions}
                          </select></div>`;
        var itmDrp = `<div class="form-group"><select class='ddlItem form-select required'>
                            <option value=0>Select Item</option>
                          </select></div>`;


        var row = myTable.row.add([
            ctgDrp,
            itmDrp,
            "<span class='price'>0</span>",
            "<input type='number' class='qty' min='1' value='1' style='width:60px' />",
            "<span class='totalAmount'>0</span>",
            "<button class='btnDelete'>Delete</button>"
        ]).draw(false).node();
        orderFormData.push({
            itemID: 0,
            categoryId: 0,
            qty: 1
        })
    });


    //on category change
    $("#myEditDataTable").off("change", ".ddlCategory").on("change", ".ddlCategory", function (e) {

        var getRow = $(this).closest('tr');
        var getIndex = getRow.index();
        var getItemDRP = getRow.find('select.ddlItem')
        var getCategoryID = $(this).val();
        orderFormData[getIndex].categoryId = getCategoryID;
        if (getCategoryID > 0) {

            var getItems = categories.find(x => x.id == getCategoryID).items;
            getItemDRP.attr("data-categoryid", getCategoryID);
            getItemDRP.html("");
            getItemDRP.append("<option value=0>Select Item</option>");

            $.each(getItems, function (index, item) {
                getItemDRP.append("<option value=" + item.id + ">" + item.name + "</option>");
            });
        }
        else {
            getItemDRP.attr("data-categoryid", 0);
            getItemDRP.html("");
            getItemDRP.append("<option value=0>Select Item</option>");
        }
    });

    //on item change
    $("#myEditDataTable").off("change", ".ddlItem").on("change", ".ddlItem", function (e) {
        var getItemId = $(this).val();
        var getCategoryId = $(this).attr("data-categoryid");
        var getRow = $(this).closest('tr');
        var getIndex = getRow.index();
        var price = 0;
        var qty = 1;
        var totalAmount = 0;
        orderFormData[getIndex].itemID = getItemId;
        if (getItemId > 0) {

            var getItemDetail = categories.find(x => x.id == getCategoryId).items.find(x => x.id == getItemId)
            price = getItemDetail.price;
            qty = getRow.find('.qty').val();
            totalAmount = price * qty;

        }
        getRow.find('.price').html(numberWithCommas(price))
        getRow.find('.totalAmount').html(numberWithCommas(totalAmount));
    });

    //on qty change
    $("#myEditDataTable").off("change", ".qty").on("change", ".qty", function () {

        var getRow = $(this).closest('tr');
        var getIndex = getRow.index();
        var getQty = $(this).val();
        var getPrice = getRow.find('.price').html();
        var totalAmount = getQty * numberWithoutCommas(getPrice);
        getRow.find('.totalAmount').html(numberWithCommas(totalAmount));
        orderFormData[getIndex].qty = getQty;
    });

    //delete row
    $("#myEditDataTable").off("click", ".btnDelete").on("click", ".btnDelete", function () {
        var getRow = $(this).closest('tr');
        var getIndex = getRow.index();
        orderFormData.splice(getIndex, 1)
        myTable.row($(this).parents('tr')).remove().draw();

    })

    //form validation
     $("#orderForm").validate({
        rules: {
            customerName: {
                required: true,
                minlength: 2
            },
            orderDate: {
                required: true,
            }
        },
        messages: {
            customerName: {
                required: "Please enter customer name"
            },
            orderDate: {
                required: "Please enter your order date"
            }
        },
        submitHandler: function (form) {

            // This function is called when the form is valid and submitted
            /*alert("Form is valid and ready to save!");*/
            if (orderFormData.length == 0) {
                alert("Add atleast one order");
            }
            else {
                if (orderFormData.some(x => x.categoryId == 0)) {
                    alert("Please select category");
                }
                else if (orderFormData.some(x => x.itemID == 0)) {
                    alert("Please select item");
                }
                else if (orderFormData.some(x => x.qty == 0).length == 0) {
                    alert("Please add at leastt one qty");
                }
                else {
                    var formData = {
                        customerName: form.customerName.value,
                        orderDate: form.orderDate.value,
                        orders: orderFormData
                    }

                    var str = JSON.stringify(formData);

                    $.ajax({
                        type: "POST",
                        url: "https://localhost:7054/api/Order/InsertOrder",
                        contentType: "application/json; charset=utf-8",
                        data: str,
                        dataType: "json",
                        success: function (response) {
                            alert(response.result);
                            $('#addOrderModal').modal('hide');
                            $("#orderForm")[0].reset();
                        },
                        error: function (response) {
                            alert(response.message)
                        },
                    })
                }
            }
        }
    });
}