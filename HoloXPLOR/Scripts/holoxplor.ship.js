﻿$(document).ready(function () {
    var debug = false;
    var $form = $('#form-ship');

    if ($form.length == 0) return;

    var acceptItem = function ($item) {
        var $port = $(this);

        // ensure we're always checking the item vs the port
        if (!$item.hasClass('js-available') && $port.hasClass('js-available')) {
            var temp = $item;
            $item = $port;
            $port = temp;
        }

        if (!$item.hasClass('js-available') && !$port.hasClass('js-available')) {
            return false;
        }

        if (!debug) { /* Check item sizes */
            var itemSize = Number.parseInt($item.data('item-size'));
            var minSize = Number.parseInt($port.data('port-min-size'));
            var maxSize = Number.parseInt($port.data('port-max-size'));

            if (itemSize < minSize || itemSize > maxSize)
                return false;
        }

        if (!debug) { /* Normally, ship-specific tags */
            var requiredPortTags = ($item.data('item-required-port-tags') || '').split(' ');
            var portTags = ($port.data('port-tags') || '').split(' ');
            var shipPortTags = ($port.data('ship-port-tags') || '').split(' ');

            if ($(requiredPortTags).not(portTags).not(shipPortTags).not(['']).length > 0)
                return false;
        }

        if (!debug) { /* Normally, equipment-specific tags */
            var requiredItemTags = ($port.data('port-required-tags') || '').split(' ');
            var itemTags = ($item.data('item-tags') || '').split(' ');

            if ($(requiredItemTags).not(itemTags).not(['']).length > 0)
                return false;
        }

        { /* Item types */
            var type = $item.data('item-type');
            var subType = $item.data('item-sub-type');
            subType = (subType == "") ? type : (type + ":" + subType);

            var accepted = ($port.data('port-types') || '').split(',');

            for (var i = 0, j = accepted.length; i < j; i++) {
                if (accepted[i] == type || accepted[i] == subType)
                    return true;
            }
        }

        return false;
    }

    var bindAll = function (parent) {
        $("[data-draggable]:not(.js-equipped):not(.js-draggable)").addClass('js-draggable').draggable({
            helper: "clone",
            appendTo: "body"
        });

        $("[data-droppable]:not(.js-droppable)").addClass('js-droppable').droppable({
            accept: acceptItem,
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover",
            drop: dropItem
        });
    }

    var dropItem = function (event, ui) {

        var $item = $(event.toElement);
        var $port = $(event.target);

        if ($item.data('item-id') != null)
            $item = $('[data-item-id="' + $item.data('item-id') + '"');

        if ($port.data('item-id') != null)
            $port = $('[data-item-id="' + $port.data('item-id') + '"');

        // ensure we're always checking the item vs the port
        if (!$item.hasClass('js-available') && $port.hasClass('js-available')) {
            var temp = $item;
            $item = $port;
            $port = temp;
        }

        var data = {
            newPartID: $item.data('item-id'),
            parentID: $port.data('parent-id'),
            portName: $port.data('port-name')
        }

        $.ajax({
            url: $form.attr('action'),
            data: data,
            method: "POST",
            success: function (data) {
                $itemPane = $item.closest('[id]');
                $portPane = $port.closest('[id]');

                $page = $(data);

                console.log($itemPane[0].id, $portPane[0].id);
                console.log($('#' + $itemPane[0].id, $page));
                console.log($('#' + $portPane[0].id, $page));

                $itemPane.replaceWith($('#' + $itemPane[0].id, $page));
                $portPane.replaceWith($('#' + $portPane[0].id, $page));

                bindAll(document.body);
            }
        });
    }

    bindAll(document.body)
});