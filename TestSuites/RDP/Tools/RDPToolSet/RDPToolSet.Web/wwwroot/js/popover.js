!function ($) {

    /* MODAL POPOVER PUBLIC CLASS DEFINITION
     * =============================== */

    var ModalPopover = function (element, options) {
        this.options = options;
        this.$element = $(element)
            .delegate('[data-dismiss="modal-popup"]', 'click.dismiss.modal-popup', $.proxy(this.hide, this));
        this.options.remote && this.$element.find('.popover-content').load(this.options.remote);
        this.$parent = options.$parent; // todo make sure parent is specified
    }; /* NOTE: MODAL POPOVER EXTENDS BOOTSTRAP-MODAL.js
     ========================================== */


    ModalPopover.prototype = $.extend({}, $.fn.modal.Constructor.prototype, {

        constructor: ModalPopover,


        getPosition: function () {
            var $element = this.$parent;
            return $.extend({}, ($element.offset()), {
                width: $element[0].offsetWidth, height: $element[0].offsetHeight
            });
        },

        show: function () {
            var $dialog = this.$element;
            $dialog.css({ top: 0, left: 0, display: 'block', 'z-index': 1050 });

            var placement = typeof this.options.placement == 'function' ?
                this.options.placement.call(this, $tip[0], this.$element[0]) :
                this.options.placement;

            var pos = this.getPosition();

            var actualWidth = $dialog[0].offsetWidth;
            var actualHeight = $dialog[0].offsetHeight;

            var tp;
            switch (placement) {
                case 'bottom':
                    tp = { top: pos.top + pos.height, left: pos.left + pos.width / 2 - actualWidth / 2 };
                    break;
                case 'top':
                    tp = { top: pos.top - actualHeight, left: pos.left + pos.width / 2 - actualWidth / 2 };
                    break;
                case 'left':
                    tp = { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth };
                    break;
                case 'right':
                    tp = { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left + pos.width };
                    break;
            }

            $dialog
                .css(tp)
                .addClass(placement)
                .addClass('in');


            $.fn.modal.Constructor.prototype.show.call(this, arguments); // super
        },

        /** todo entire function was copied just to set the background to 'none'.  need a better way */
        backdrop: function (callback) {
            var that = this
                , animate = this.$element.hasClass('fade') ? 'fade' : '';
            if (this.isShown && this.options.backdrop) {
                var doAnimate = $.support.transition && animate;
                this.$backdrop = $('<div class="modal-backdrop ' + animate + '" style="background:none" />')
                    .appendTo(document.body);
                if (this.options.backdrop != 'static') {
                    this.$backdrop.click($.proxy(this.hide, this));
                }

                if (doAnimate) this.$backdrop[0].offsetWidth; // force reflow

                this.$backdrop.addClass('in');
                doAnimate ?
                    this.$backdrop.one($.support.transition.end, callback) :
                    callback();
            } else if (!this.isShown && this.$backdrop) {
                this.$backdrop.removeClass('in');
                $.support.transition && this.$element.hasClass('fade') ?
                    this.$backdrop.one($.support.transition.end, $.proxy(this.removeBackdrop, this)) :
                    this.removeBackdrop();
            } else if (callback) {
                callback();
            }
        }

    });


    /* MODAL POPOVER PLUGIN DEFINITION
     * ======================= */

    $.fn.modalPopover = function (option) {
        return this.each(function () {
            var $this = $(this);
            var data = $this.data('modal-popover');
            var options = $.extend({}, $.fn.modalPopover.defaults, $this.data(), typeof option == 'object' && option);
            // todo need to replace 'parent' with 'target'
            options['$parent'] = (data && data.$parent) || option.$parent || $(options.target);

            if (!data) $this.data('modal-popover', (data = new ModalPopover(this, options)));
            if (typeof option == 'string') data[option]();
        });
    };
    $.fn.modalPopover.Constructor = ModalPopover;
    $.fn.modalPopover.defaults = $.extend({}, $.fn.modal.defaults, {
        placement: 'right',
        keyboard: true
    });


    $(function () {
        $('body').on('click.modal-popover.data-api', '[data-toggle="modal-popover"]', function (e) {
            var $this = $(this);
            var href = $this.attr('href');
            var $dialog = $($this.attr('data-target') || (href && href.replace(/.*(?=#[^\s]+$)/, ''))); //strip for ie7
            var option = $dialog.data('modal-popover') ? 'toggle' : $.extend({ remote: !/#/.test(href) && href }, $dialog.data(), $this.data());
            option['$parent'] = $this;

            e.preventDefault();

            $dialog
                .modalPopover(option)
                .modalPopover('show')
                .one('hide', function () {
                    $this.focus();
            });
        });
    });
}(window.jQuery);
//