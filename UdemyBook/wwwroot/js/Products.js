var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url":"/Admin/Products/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "author.name", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "price", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                        <a href="/Admin/Products/Upsert/${data}" class="btn btn-pink text-white" style="cursor:pointer">
                            <i class="fa fa-edit"></i>
                        </a>
                        <a onclick=Delete("/Admin/Products/Delete/${data}") class="btn btn-dark text-white" style="cursor:pointer">
                            <i class="fa fa-trash"></i>
                        </a>
                    </div>
                    `;
                },
                "width":"25%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete ?",
        text: "You will not be able to restore the data",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}