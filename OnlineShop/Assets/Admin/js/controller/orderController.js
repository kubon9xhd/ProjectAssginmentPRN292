$(document).ready(function () {
    $('button#active').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        var btn = $(this);
        var id = btn.data("id");
        console.log(id);
        $.ajax({
            url: "/Admin/Order/ChangeStatus",
            data: {
                id: id
            },
            dataType: "json",
            method: "POST",
            success: function (response) {
                if (response.status == true) {
                    btn.text("Delivered");
                    btn.removeClass("btn btn-danger");
                    btn.addClass("btn btn-success");
                } else {
                    btn.text("Pending");
                    btn.removeClass("btn btn-success");
                    btn.addClass("btn btn-danger");
                }
            }
        })
    });
})
function confirmBeforeAfterDelete(data) {
    Swal.fire({
        title: "Are you want to delete?",
        text: 'You will not be able to recover this imaginary Category!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, keep it'
    }).then((result) => {
        if (result.isConfirmed) {
            remove(data);
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            Swal.fire(
                'Cancelled',
                'Your imaginary account is safe :)',
                'error'
            )
        }
    });
}
function remove(data) {
    $.ajax({
        url: "/Admin/Order/Delete",
        type: "Delete",
        data: {
            id: data
        },
        success: function (e) {
            Swal.fire(
                'Deleted!',
                'Delete successfully.',
                'success'
            );
            $("#row_" + data + "").remove();
        },
        error: function (e) {
            Swal.fire(
                'Deleted',
                'CAn not delete this account',
                'error'
            )
        }
    })
}