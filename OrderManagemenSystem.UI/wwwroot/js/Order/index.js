
$(document).ready(function () {

    var categories = [];
    getCategory(function (getCategories) {
        categories = getCategories;
        var orderDetailTable = $('#myDataTable').DataTable({
            processing: true,
            serverSide: true,
            paging: true,
            searching: true,
            info: true,
            order: [[1, 'asc']],
            "ajax": function (data, callback, settings) {
                if (data.order.length == 0) {
                    data.order.push({
                        name: "Date",
                        dir: "asc",
                        column: 1,
                    })
                }
                $.ajax({
                    url: "https://localhost:7054/api/Order/GetOrderDetails",
                    method: "POST",
                    timeout: 0,
                    headers: {
                        "accept": "*/*",
                        "Content-Type": "application/json"
                    },
                    data: JSON.stringify({
                        "searchText": data.search.value,
                        "sortColumn": data.order[0].name,
                        "sortType": data.order[0].dir,
                        "pageNumber": (data.start / data.length) + 1,
                        "pageSize": data.length
                    }),
                    success: function (response) {
                        var result = JSON.parse(response.result);

                        callback({
                            recordsTotal: result.totalCount,
                            recordsFiltered: result.totalCount,
                            data: result.data
                        });
                    }
                })
            },
            columnDefs: [
                {
                    targets: [0],
                    orderable: false
                },
                {
                    targets: [1],
                    render: DataTable.render.datetime('DD/MM/YYYY')
                },
                {
                    targets: [8],
                    orderable: false
                }
            ],
            columns: [
                { "data": "orderNo", name: "OrderNo" },
                { "data": "date", name: "Date" },
                { "data": "customerName", name: "CustomerName" },
                { "data": "cateoryName", name: "CateoryName" },
                { "data": "itemName", name: "ItemName" },
                { "data": "qty", name: "Qty" },
                {
                    "data": "price", name: "Price", render: function (data, type, row, meta) {
                        if (type == "display") {
                            return numberWithCommas(data);
                        }
                        return data;
                    }
                },
                {
                    "data": "totalAmount", name: "TotalAmount", render: function (data, type, row, meta) {
                        if (type == "display") {
                            return numberWithCommas(data);
                        }
                        return data;
                    }
                },
                {
                    "data": "id", name: "OrderNo", render: function (data, type, row, meta) {
                        return `<a href="javascript:void(0)" class="lnkEdit" data-toggle="modal" data-target="#editOrderModal" data-id=${data}><i class="fa fa-pencil-square-o text-info" style="font-size:24px"></i></a>
                            <a href="javascript:void(0)" class="lnkDelete" data-id=${data}><i class="ml-5 fa fa fa-trash-o text-danger" style="font-size:24px;margin-left: 5px"></i></a>`;
                    }
                }
            ]
        });


        //delete data
        $("#myDataTable").on('click', '.lnkDelete', function (e) {
            var id = $(this).attr("data-id")
            if (confirm("Are you sure you want to delete this item?")) {

                $.ajax({
                    url: 'https://localhost:7054/api/Order/DeleteOrder?id=' + id,
                    type: 'DELETE',
                    success: function (response) {
                        alert(response.result);
                        orderDetailTable.draw();
                    },
                    error: function (response) {
                        alert(response.message);
                        orderDetailTable.draw();
                    }
                });
            }
        });

        //update data
        $("#myDataTable").on('click', '.lnkEdit', function (e) {
            var id = $(this).attr("data-id");
            $.ajax({
                url: "https://localhost:7054/api/Order/GetOrder?id=" + id,
                type: 'GET',
                dataType: 'json',
                success: function (response) {
                    
                    var result = JSON.parse(response.result);

                    $("#editOrderModal").modal('show');
                    EditOrderDetails(result, categories);
                }, error: function (jqXHR, textStatus, errorThrown) {
                    callback([]);
                }
            });
        });

        $("#btnAddNewOrder").click(function (e) {
            $('#addOrderModal').modal('show');
        });

        $('#addOrderModal').on('shown.bs.modal', function () {
            if ($.fn.DataTable.isDataTable('#myEditDataTable')) {
                $('#myEditDataTable').DataTable().clear().destroy();
            }
            if ($("#orderForm").data("validator")) {
                $("#orderForm").validate().destroy();
            }
            editableData(categories);
        })

        $("#addOrderModal").on('hide.bs.modal', function () {
            if ($.fn.DataTable.isDataTable('#myEditDataTable')) {
                $('#myEditDataTable').DataTable().clear().destroy();
            }
            if ($("#orderForm").data("validator")) {
                $("#orderForm").validate().destroy();
            }
            orderDetailTable.draw();
        });

        $("#addOrderModal .close").on('click', function () {
            $("#addOrderModal").modal('hide')

            $("#orderForm")[0].reset()
            orderDetailTable.draw();
        });

        $("#editOrderModal").on('click', ".close", function () {
            if ($("#editOrderForm").data("validator")) {
                $("#editOrderForm").validate().destroy();
            }
            $("#editOrderModal").modal('hide')
            orderDetailTable.draw();
        });

    })

    function getCategory(callback) {

        var categories;
        $.ajax({
            url: "https://localhost:7054/api/Order/GetAllCategories",
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                categories = JSON.parse(result.result);
                callback(categories)
            }, error: function (jqXHR, textStatus, errorThrown) {
                callback([]);
            }
        });
    }
    
});

function numberWithCommas(num) {
    num = parseFloat(num).toFixed(2);
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function numberWithoutCommas(num) {
    return num.toString().replace(",", "");
}