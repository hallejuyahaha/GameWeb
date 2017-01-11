var a = document.getElementById("uname_text");
var b = document.getElementById("pword_text");
var main = document.getElementById("main");
var zz = document.getElementById("zz");
var newuser = document.getElementById("newuser");
var newpassword = document.getElementById("newpassword");

function zhu() {
    main.style.display = "none";
    zz.style.display = "block";
}
function aaaa() {
    main.style.display = "block";
    zz.style.display = "none";
}
function add() {
    
    $.ajax({
        url: '/hand.ashx',
        dataType: 'text',
        type: 'POST',
        data: {
            newname: newuser.value,
            newpass: newpassword.value,
            method: "indexAdd",
        },
        success: function (out) {
            if (out == "成功") {
                window.alert("注册成功");
                aaaa();
            } else if (out == "失败") {
                window.alert("用户名已存在");
            }
        },
        error: function () { alert('error 注册'); }
    });
    
}