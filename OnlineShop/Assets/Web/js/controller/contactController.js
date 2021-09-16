$(document).ready(function () {
    $('#btnSubmit').off('click').on('click', function () {
        var name = $('#txtName').val();
        var email = $('#txtEmail').val();
        var phone = $('#txtPhone').val();
        var address = $('#txtAddress').val();
        var content = $('#txtContent').val();
        $.ajax({
            url: '/contact/send',
            type: 'post',
            dataType: 'json',
            data: {
                name: name,
                email: email,
                phone: phone,
                addres: address,
                content: content
            },
            success: function (res) {
                if (res.status == true) {
                    ShowMessage("Send email done", "success");
                    $('#txtName').val('');
                    $('#txtEmail').val('');
                    $('#txtPhone').val('');
                    $('#txtAddress').val('');
                    $('#txtContent').val('');
                } else {
                    ShowMessage("Send email fail", "warning");
                }
            }
        })
    });
})

function ShowMessage(mess, code) {
    Lobibox.notify(code, {
        delay: 10000,
        msg: mess
    });
}