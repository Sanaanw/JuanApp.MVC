$(document).ready(function () {
    $('.addtobasket').click(function (e) {
        e.preventDefault();
        let url = $(this).attr('href');
        alert(url);
        fetch(url)
            .then(response => response.text())
            .then(data => {
                location.reload();
            });
    })
})
