$(document).ready(function () {
    $('#btnContinue').off('click').on('click', function (e) {
        window.location.href = "/";
    });
    $('#btnCheckOut').off('click').on('click', function (e) {
        window.location.href = "/check-out"
    });
    $('#btnUpdateCart').off('click').on('click', function (e) {
        var listProduct = $('.txtQuantity');
        var cartList = [];
        $.each(listProduct, function (i, item) {
            var size = $('.txtSize')[i].value;
            var code = $('.codeProduct')[i].textContent;
            var valueItem = $(item).val();
            if (valueItem <= 0) {
                Lobibox.notify('error', {
                    delay: 10000,
                    msg: 'The quantity of ' + code + ' can not less than 0'
                });
            } else {
                if (size == "s" || size == "l" || size == "m" || size == "xl" || size == "xxl") {
                    cartList.push({
                        Quantity: $(item).val(),
                        Product: {
                            ID: $(item).data('id')
                        },
                        size: size
                    })
                } else {
                    Lobibox.notify('error', {
                        delay: 10000,
                        msg: 'The size of ' + code.val() + ' not right'
                    });
                }
            }
        })
        $.ajax({
            url: '/cart/update',
            data: { cartModel: JSON.stringify(cartList) },
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status) {
                    window.location.href = "/cart";
                } else {
                    Lobibox.notify('error', {
                        delay: 15000,
                        msg: 'Can not update cart'
                    });
                }
            }
        })
    });
    $('#btnDeleteAll').off('click').on('click', function (e) {
        $.ajax({
            url: '/Cart/DeleteAll',
            type: 'POST',
            success: function (res) {
                if (res.status) {
                    window.location.href = "/cart";
                } else {
                    Lobibox.notify('error', {
                        delay: 15000,
                        msg: 'Can not delete cart'
                    });
                }
            }
        })
    });
    $('.btn-delete').off('click').on('click', function (e) {
        $.ajax({
            url: '/Cart/Delete',
            data: {
                id: $(this).data('id'),
                size: $(this).data('size')
            },
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status) {
                    window.location.href = "/cart";
                } else {
                    Lobibox.notify('error', {
                        delay: 15000,
                        msg: 'Can not delete cart'
                    });
                }
            }
        })
    });
    $('#btnPlaceOrder').off('click').on('click', function () {
        var name = $("input[name=shipName]").val();
        if (name == "") {
            ShowMessage("Name can not empty", "warning");
            return false;
        }
        var email = $("input[name=shipEmail]").val();
        if (email == "") {
            ShowMessage("Email can not empty", "warning");
            return false;
        }
        var mobile = $("input[name=shipMobile]").val();
        if (mobile == "") {
            ShowMessage("Mobile can not empty", "warning");
            return false;
        }
        if (!isPhoneNumber(mobile)) {
            ShowMessage("Please enter right mobile number", "warning");
            return false;
        }
        var addresss = $("input[name=shipAddress]").val();
        if (addresss == "") {
            ShowMessage("Address can not empty", "warning");
            return false;
        }
        var country = $("#custom-select :selected").text;
        if (country == "") {
            ShowMessage("Name can not empty", "warning");
            return false;
        }
        var city = $("input[name=shipCity]").val();
        if (city == "") {
            ShowMessage("City can not empty", "warning");
            return false;
        }
        var district = $("input[name=shipDistrict]").val();
        if (district == "") {
            ShowMessage("District can not empty", "warning");
            return false;
        }
        var village = $("input[name=shipVillage]").val();
        if (village == "") {
            ShowMessage("Village can not empty", "warning");
            return false;
        }
        
        var OrderDetail = [];
        OrderDetail.push({
            ShipName: name,
            ShipEmail: email,
            ShipMobile: mobile,
            ShipAddress: addresss,
            ShipCountry: country,
            ShipCity: city,
            ShipDistrict: district,
            ShipVillage: village
        });
        $.ajax({
            url: '/check-out',
            data: { OrderDetailModel: JSON.stringify(OrderDetail) },
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status) {
                    window.location.href = "/order-detail/"+res.code;
                } else {
                    Lobibox.notify('error', {
                        delay: 15000,
                        msg: 'Can not check-out cart'
                    });
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
function isPhoneNumber(inputtxt) {
    var phoneno = /^\d{10}$/;
    if (inputtxt.match(phoneno)) {
        return true;
    }
    else {
        return false;
    }
}