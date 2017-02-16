$(function () {
    var name = localStorage.getItem('name');
    var role = localStorage.getItem('role');
    var roleid = localStorage.getItem('roleid');
    $('header .dropdown').find('span').eq(0).text(name);
    $('header .dropdown').find('span').eq(1).text(role);
    $('header .dropdown').find('img').attr('src', '/task/asset/img/user' + roleid + '.jpg');
});