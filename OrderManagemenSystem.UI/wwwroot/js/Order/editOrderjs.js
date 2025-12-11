function EditOrderDetails(orderDetail, categories) {
    //set value
    var getInputs = $("#editOrderForm input");
    $.each(getInputs, function (index, item) {
        var getName = $(item).attr('name');
        if ($(item).attr('type') != 'submit') {

            if (getName == 'orderDate') {
                getName = 'date'
            }
            $(item).val(orderDetail[getName]);
        }
    })

   //bind category
    var categoryOptions = "<option value=0>Select Category</option>";
    $.each(categories, function (index, item) {
        categoryOptions += `<option value="${item.id}" ${item.id == orderDetail.categoryId ? "selected" : ""} > ${item.name}</option >`;
    });
    $("#categoryId").html("");
    $("#categoryId").append(categoryOptions);

    //bind items
    var getItems = categories.find(x => x.id == orderDetail.categoryId).items;
    var itemOptions = "<option value=0>Select Item</option>"

    $.each(getItems, function (index, item) {
        itemOptions += `<option value="${item.id}" ${item.id == orderDetail.itemId ? "selected" : ""}>${item.name}</option>`;
    });
    $("#itemId").attr("data-categoryid", orderDetail.categoryId)
    $("#itemId").append(itemOptions);

    // on category change
    $("#editOrderModal").off("change", "#categoryId").on("change", "#categoryId", function (e) {
        var getCategoryID = $(this).val();
        var getItemDRP = $("#itemId");
        var price = 0;
        var qty = 1;
        var totalAmount = 0;
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

        $("#price").val(numberWithCommas(price)).trigger('change')
        $('#qty').val(1).trigger('change')
        $("#itemId").trigger('change');
        $("#totalAmount").val(numberWithCommas(totalAmount)).trigger('change');
    });

    //on item change
    $("#editOrderModal").off("change", "#itemId").on("change", "#itemId", function (e) {
        var getItemId = $(this).val();
        var getCategoryId = $(this).attr("data-categoryid");
        var price = 0;
        var qty = 1;
        var totalAmount = 0;
        if (getItemId > 0) {

            var getItemDetail = categories.find(x => x.id == getCategoryId).items.find(x => x.id == getItemId)
            price = getItemDetail.price;
            qty = $('#qty').val();
            totalAmount = price * qty;

        }
        $("#price").val(numberWithCommas(price)).trigger('change')
        $("#totalAmount").val(numberWithCommas(totalAmount)).trigger('change')
    });

    //on qty change
    $("#editOrderModal").off("change", "#qty").on("change", "#qty", function (e) {
        var price = numberWithoutCommas($("#price").val());
        var qty = $(this).val();
        var totalAmount = price * qty;
        $("#totalAmount").val(numberWithCommas(totalAmount)).trigger('change')
    })

    //form validation
    $.validator.addMethod("valueNotEquals", function (value, element, arg) {
        return arg !== value;
    }, "Please select an option.");
    var editOrderValidator = $("#editOrderForm").validate({
        rules: {
            customerName: {
                required: true,
                minlength: 2
            },
            orderDate: {
                required: true,
            },
            categoryId: {
                required: true,
                valueNotEquals: "0"
            },
            itemId: {
                required: true,
                valueNotEquals: "0"
            },
            qty: {
                required: true,
                min: 1,
                number: true
            }
        },
        messages: {
            customerName: {
                required: "Please enter customer name"
            },
            orderDate: {
                required: "Please enter your order date"
            },
            categoryId: {
                required: "Please select category",
                valueNotEquals: "Please select category"
            },
            itemId: {
                required: "Please Select item ",
                valueNotEquals: "Please select item"
            },
            qty: {
                required: "Please enter your qty",
                min: "Please add atleast one qty"
            }
        },
        submitHandler: function (form) {
            var formData = {
                customerName: form.customerName.value,
                orderDate: form.orderDate.value,
                itemID: form.itemId.value,
                categoryId: form.categoryId.value,
                qty: form.qty.value,
                id: form.id.value
            }

            var str = JSON.stringify(formData);

            $.ajax({
                type: "PUT",
                url: "https://localhost:7054/api/Order/UpdateOrder",
                contentType: "application/json; charset=utf-8",
                data: str,
                dataType: "json",
                success: function (response) {
                    alert(response.result);
                    $('#editOrderModal').modal('hide');
                    $("#editOrderForm")[0].reset();
                },
                error: function (response) {
                    alert(response.responseJSON.message)
                },
            })

        }
    });

}