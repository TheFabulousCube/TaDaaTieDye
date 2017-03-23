// JavaScript source code

function menu() {
    $(".flyOutMenu").accordion({
        event: "mouseover"
    });
    $(".flyOutMenu").accordion("option", "collapsible", true);
}
    menu();