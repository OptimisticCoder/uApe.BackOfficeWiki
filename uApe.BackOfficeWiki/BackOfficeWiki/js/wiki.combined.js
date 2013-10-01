var adjustment;
var itemHeight;

$(function () {
    var oldContainer;

    if ($("#" + hdnMode).val() != "Edit")
        $(".editorIconSplit").hide();
    $("ul.categorylist").sortable({
        containerSelector: 'ul',
        group: 'root',
        nested: false,
        pullPlaceholder: false,
        onDragStart: function (item, container, _super) {
            var offset = $(item).offset(),
                pointer = container.rootGroup.pointer;

            itemHeight = $(item).outerHeight();
            console.log(itemHeight);
            adjustment = {
                left: pointer.left - offset.left,
                top: pointer.top - offset.top
            };

            _super(item, container);
        },
        onDrag: function (item, position) {
            $(item).css("left", position.left - adjustment.left);
            $(item).css("top", position.top - adjustment.top);
            $(".placeholder").css("height", itemHeight);
            $(".dragged").css("height", itemHeight);
        },
        isValidTarget: function (item, container) {
            if ($(item).parent().hasClass("categorylist"))
                return true;
            return false;
        },
        tolerance: 6,
        distance: 10,
        afterMove: function (placeholder, container) {
            if (oldContainer != container) {
                if (oldContainer)
                    oldContainer.el.removeClass("active");
                container.el.addClass("active");

                oldContainer = container;
            }
        },
        onDrop: function (item, container, _super) {
            container.el.removeClass("active");
            _super(item);
        }
    });
    $("ul.categorylist ol").sortable({
        containerSelector: 'ol',
        group: 'nested',
        nested: false,
        pullPlaceholder: false,
        onDragStart: function (item, container, _super) {
            var offset = $(item).offset(),
                pointer = container.rootGroup.pointer;

            itemHeight = $(item).outerHeight();
            console.log(itemHeight);
            adjustment = {
                left: pointer.left - offset.left,
                top: pointer.top - offset.top
            };

            _super(item, container);
        },
        onDrag: function (item, position) {
            $(item).css("left", position.left - adjustment.left);
            $(item).css("top", position.top - adjustment.top);
            $(".placeholder").css("height", itemHeight);
            $(".dragged").css("height", itemHeight);
        },
        isValidTarget: function (item, container) {
            if (!$(item).parent().hasClass("categorylist"))
                return true;
            return false;
        },
        tolerance: 6,
        distance: 10,
        afterMove: function (placeholder, container) {
            if (oldContainer != container) {
                if (oldContainer)
                    oldContainer.el.removeClass("active");
                container.el.addClass("active");

                oldContainer = container;
            }
        },
        onDrop: function (item, container, _super) {
            container.el.removeClass("active");
            _super(item);
        }
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
    // build json data
    var displayOrder = "";
    $("#categorylist>li>strong").each(function () {
        var catName = $(this).text().trim();
        var pages = "";
        $(this).parent().find("ol>li").each(function () {
            if (pages.length === 0)
                pages = "['" + $(this).find("span").text() + "'";
            else
                pages += ", '" + $(this).find("span").text() + "'";
        });
        if (pages.length === 0)
            pages = "[]";
        else
            pages += "]";

        if (displayOrder.length === 0)
            displayOrder = "{ cats: [{ catName: '" + catName + "', pages: " + pages + "}";
        else
            displayOrder += ", { catName: '" + catName + "', pages: " + pages + "}";
    });
    if (displayOrder.length === 0)
        displayOrder = "{ cats: []}";
    else
        displayOrder += "]}";

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
