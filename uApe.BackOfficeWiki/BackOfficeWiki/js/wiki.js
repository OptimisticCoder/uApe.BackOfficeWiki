
$(function () {
    $("#categorylist").sortable({
        group: 'cats',
        handle: 'img.drag-handle',
        pullPlaceholder: true
    });
});

function deleteCategory() {
    if ($("#" + ddlDelCatName + " option:selected").val() !== "") {
        $("#delCatModalTitle").text("Delete Category: " + $("#" + ddlDelCatName + " option:selected").text());
        $('#delCatModal').modal();
    }
    return false;
}

function saveCatOrder() {

    var displayOrder = "";
    $("#categorylist li h5").each(function () {
        if (displayOrder.length === 0)
            displayOrder = $(this).text().trim();
        else
            displayOrder += "|,|" + $(this).text().trim();
    });
    $("#" + hdnDisplayOrder).val(displayOrder);
    return true;
}

function deletePage() {
    if ($("#" + hdnPageName).val() !== "") {
        $("#delPageModalTitle").text("Delete Page: " + $("#" + hdnPageName).val());
        $('#delPageModal').modal();
    }
    return false;
}
