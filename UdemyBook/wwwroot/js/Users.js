var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url":"/Admin/Users/GetAll"
        },
        "columns": [
            { "data": "name" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "address" },
            { "data": "city" },
            { "data": "role" },
            {
                "data": {
                    id: "id",
                    lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fa fa-unlock"></i>
                                </a>
                            </div>
                            `;
                    }
                    else {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-dark text-white" style="cursor:pointer">
                                    <i class="fa fa-lock"></i>
                                </a>
                            </div>
                            `;
                    }
                    
                }
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/Users/LockUnlock",
        data: JSON.stringify(id), //pass id đi
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            } else {
                toastr.error(data.message);
            }
        }
    });
}