$(document).ready(function () {
    $(".addToBasket").click(function (e) {
        e.preventDefault();
        let productId = $(this).attr("data-id");
        console.log(productId)

        fetch("/Home/AddToBasket/" + productId).then(res => {
            if (res.ok) {
                alert("oldu")
                return res.text();
            }
            else {
                alert("olmadi")
            }
        }).then(data =>
            $(".basketUl").html(data)
        );
    })
    $(".number .up").click(function salam() {
        console.log(1)
        //e.preventDefault();
        let productId = $(this).attr("data-rid");
        let count = Number($(this).next().text());
        count += 1;
        //fetch("/Basket/ChangeProductCount/" + productId + '?count=' + count).then(res => {
        //    return res.text();
        //}).then(data => {
        //    $(this).next().html(data);
        //})
        $(this).prev().html(count)

    })
    
    //$(".item-count .number .down").click(function (e) {
    //    e.preventDefault();
    //    let productId = $(this).attr("data-id");
    //    let count = Number($(this).prev().text());
    //    if (count != 0) {
    //        count -= 1;
    //    }
    //    else {
    //        count = 0;
    //    }
    //    fetch("/Basket/ChangeProductCount/" + productId + "?count=" + count).then(res => { return res.text(); }).then(data => { $(".cart-body").html(data); })
    //    //$(this).prev().html(count)
    //})
});