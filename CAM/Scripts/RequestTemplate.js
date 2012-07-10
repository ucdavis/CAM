$(function () {

    // if uncheck available, selected needs to be unselected
    $(".available").change(function () {
        if (!$(this).is(":checked")) {
            $(this).closest("tr").find(".selected").attr("checked", false);
        }
    });

    // uncheck selected, available shouldn't be selected
    $(".selected").change(function () {
        if ($(this).is(":checked")) {
            $(this).closest("tr").find(".available").attr("checked", true);
        }
    });
});