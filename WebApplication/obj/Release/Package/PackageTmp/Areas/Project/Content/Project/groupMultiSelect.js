(function ($) {
    $.fn.groupMultiSelect = function (config) {

        var self = this;
        self.settings = $.extend({
            // These are the defaults.
            options: null,
            placeholder: "Group Select",
            onChange: function (parent, children) {
            },
            initial: null
        }, config);
        self.unique_id = (new Date()).getTime();
        var html = "<div class='groupSelect'><div class='select-arrow-down'></div><div class='select'><div class='placeholder'> " + self.settings.placeholder + "</div></div><div class='options group-select-hide'>";

        this.generateHTML = function (options) {
            var html_options = '';
            $.each(options, function (i) {

                var id_parent = this.label.toLowerCase().replace(/ /g, '-')
                html_options += "<div style=" + "'margin-bottom:-13px;'" + "><input type='radio' name='parent_" + self.unique_id + "' class='group-select-option' id='id_parent_" + self.unique_id + "_" + i + "' value='" + this.label + "' Ids='" + this.Ids + "'><label  style=" + "'font-size: 15px;margin-left:10px;font-weight:700;'" + " for='id_parent_" + self.unique_id + "_" + i + "'>" + this.label + "</label>";
                if (this.subItems.length > 0) {
                    html_options += "<div  style=" + "'margin-top:-10px;'" + " class='subitem-group " + id_parent + "' style='display: none'>";
                    $.each(this.subItems, function (index) {
                        html_options += "<div class='subitem' style=" + "'margin-bottom:-13px;'" + "><input type='radio' name='child' id='subitem_" + id_parent + "_" + index + "' class='group-select-option-subitem' data-parent='" + id_parent + "' value='" + this.label + "' Ids='" + this.Ids + "' ><label  style=" + "'font-size: 15px;margin-left:10px;font-weight:700;'" + " for='subitem_" + id_parent + "_" + index + "'>" + this + "</label></div>";
                    })
                    html_options += "</div>";
                }
                html_options += "</div>";
            });
            return html_options;
        }

        if (!self.settings.options) {

            var groups = []
            $(self).find("select").find("optgroup").each(function () {
                var group_options = [];
                $(this).find('option').each(function () {
                    group_options.push({ label: $(this).val(), Ids: $(this).attr('data-Id') });
                })

                groups.push({
                    label: $(this).attr('label'),
                    Ids: $(this).attr('data-Id'),
                    subItems: group_options
                });
            });
            self.settings.options = groups;
        }
        html += this.generateHTML(self.settings.options);
        html += "</div></div></div>"
        $(this).html(html);

        return this.each(function () {

            $(document).click(function (event) {
                if (!$(self).find(event.target).closest('.groupSelect').length) {
                    $(self).find(".options").slideUp('fast');
                } else {
                    if ($(self).find(event.target).closest('.select').length) {
                        $(self).find(".options").slideToggle('fast');
                    } else {
                        $(self).find(".options").slideDown('fast');
                    }
                }
            });
            $(self).find("input[name^='parent_']").on('change', function () {

                var valor = $(this).val();

                var parent = valor.toLowerCase().replace(/ /g, '-');
                $(self).find('.' + parent).css('display', 'block');
                $(self).find('.subitem-group').each(function () {
                    if (!$(this).hasClass(parent)) {
                        $(this).css('display', 'none');
                    }
                });
                $(self).find('.select').html('<div class="group-selection"><div class="parent">' + valor + '</div></div>')
                $(self).find('.options').find('[data-parent="' + parent + '"]').each(function () {
                    $(this).prop('checked', false);
                });
                self.settings.onChange(valor, []);
            });

            $(self).find(".group-select-option-subitem").on('change', function () {

                var parent = $(this).data('parent')
                var parent_label = $(self).find('.select').find('.group-selection').find(".parent").text()
                var children = []
                $(self).find('.' + parent).find("[data-parent='" + parent + "']").each(function () {
                    if ($(this).is(':checked') == true) {
                        children.push($(this).val());
                    }
                });
                var selected = "";
                $.each(children, function () {
                    selected += "<div class='subitem-label'>" + this + "</div>";
                });

                $(self).find('.select').find('.group-selection').find('.children').html(selected);

                self.settings.onChange($(self).find('.select').find('.group-selection').find('.parent').text(), children);
            });
            if (self.settings.initial) {
                $.each(self.settings.options, function () {
                    if (this.label == self.settings.initial.parent) {
                        $(self).find("input[value='" + self.settings.initial.parent + "']").prop("checked", true);
                        $(self).find("input[value='" + self.settings.initial.parent + "']").trigger('change');
                        var subItems = this.subItems;
                        $.each(self.settings.initial.children, function (index, value) {
                            //if(subItems.indexOf(value)!=-1){
                            //    $(self).find("input[value='"+value+"']").prop("checked", true);
                            //    $(self).find(".group-select-option-subitem").trigger('change');
                            //}
                            if (self.settings.initial.children[index] == value) {
                                $(self).find("input[value='" + value + "']").prop("checked", true);
                                $(self).find(".group-select-option-subitem").trigger('change');
                            }

                        })
                    }
                })
            }
        })
    };
})(jQuery);
