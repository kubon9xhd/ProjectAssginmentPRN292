$(document).ready(function () {
    $('#btnUpdateAccount').off('click').on('click', function (e) {
        var username = $('#txtUsername').val();
        var name = $('#txtName').val();
        var address = $('#txtAddress').val();
        var mobile = $('#txtMobile').val();
        var email = $('#txtEmail').val();
        if (username == "") {
            ShowMessage("Name can not empty", "error");
            return;
        }
        if (email == "") {
            ShowMessage("Email can not empty", "error");
            return;
        }
        $.ajax({
            url: '/user/update',
            type: 'post',
            dataType: 'json',
            data: {
                username: username,
                name: name,
                email: email,
                phone: mobile,
                addres: address
            },
            success: function (res) {
                if (res.status == true) {
                    ShowMessage(res.message, "success");
                } else {
                    ShowMessage(res.message, "warning");
                }
            }
        })
    });
    $('#btnSaveChange').off('click').on('click', function (e) {
        var oldPassword = $('#oldPassword').val();
        var newPassword = $('#newPassword').val();
        var reNewPassword = $('#reNewPassword').val();
        if (oldPassword == "") {
            ShowMessage("Old Password can not empty", "error");
            return;
        }
        if (newPassword == "") {
            ShowMessage("New Password can not empty", "error");
            return;
        }
        if (reNewPassword == "") {
            ShowMessage("Re New Password can not empty", "error");
            return;
        }
        $.ajax({
            url: '/user/changepassword',
            type: 'post',
            dataType: 'json',
            data: {
                oldPassword: oldPassword,
                newPassword: newPassword
            },
            success: function (res) {
                if (res.status == true) {
                    ShowMessage(res.message, "success");
                } else {
                    ShowMessage(res.message, "warning");
                }
            }
        })
    })
})
function ShowMessage(mess, code) {
    Lobibox.notify(code, {
        delay: 10000,
        msg: mess
    });
}