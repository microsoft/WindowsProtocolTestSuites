$(function () {
    var ENCODED_IMAGE = "EncodedImage";
    var PREVIOUS_FRAME_IMAGE = "PreviousFrameImage";

    // expand the first accordion panel
    $('.panel-group .panel-heading:first').find('a').click();

    // show quantization factors as a modal popover
    $('.quant-popover').modalPopover({
        target: '.quant-setting',
        placement: 'top'
    });

    // show popover on click
    $('.quant-setting').click(function () {
        $('.quant-popover').modalPopover('toggle');
    });

    // make quantization factors into sliders
    $('.quant-factor-slider').slider();
    $('.slider-horizontal').width('100%');

    //initalize the ZeroClipboard
    var currentURL = location.href,
        swfPath = currentURL + "/../Static/ZeroClipboard.swf";
    ZeroClipboard.config({ swfPath: swfPath });
    var client = new ZeroClipboard($('.btn-copy'));
    AddClipboardListener();

    function AddClipboardListener() {
        client.on("copy", function (event) {
            var clipboard = event.clipboardData;
            var id = event.target.id;
            var content = $('#' + id).parents('.panel .tab-content').find('.content.output-tab-pane .tab-data-content.tab-pane.active .well');
            if (content.size() > 1) {
                content = $('#' + id).parents('.panel .tab-content').find('.content.output-tab-pane .tab-pane.layer-panes.active .tab-data-content.tab-pane.active .well');
            }
            var text = content.text();
            clipboard.setData("text/plain", text);
        });
    }

    // Configurate the DropZone
    Dropzone.options.dropUploader = {
        maxFilesize: 1,
        addRemoveLinks: true,
        maxFiles: 1,
        init: function () {
            this.on("removedfile", function () {
                $.ajax({
                    type: 'GET',
                    url: location.pathname + "/RemoveUploadedImage?imageType=" + ENCODED_IMAGE
                });
            });
        }
    };

    // Configurate the PreFrame DropZone
    Dropzone.options.previousFrameUploader = {
        maxFilesize: 1,
        addRemoveLinks: true,
        maxFiles: 1,
        init: function () {
            this.on("removedfile", function () {
                $.ajax({
                    type: 'GET',
                    url: location.pathname + "/RemoveUploadedImage?imageType=" + PREVIOUS_FRAME_IMAGE
                });
            });
        }
    };

    // Configurate the Image Compare Uploader DropZone 1
    Dropzone.options.ImageCompareUploader1 = {
        maxFilesize: 1,
        addRemoveLinks: true,
        maxFiles: 1,
    };

    // Configurate the Image Compare Uploader DropZone 2
    Dropzone.options.ImageCompareUploader2 = {
        maxFilesize: 1,
        addRemoveLinks: true,
        maxFiles: 1,
    };

    // retrieve the entropy algorithm value
    function getEntropyAlgorithm(query) {
        $panelGroup = $(query).parents('.panel-group');
        return {
            Algorithm: $panelGroup.find('.entropy-alg').val()
        };
    }

    // retrieve the quantization factor values
    function getQuantizationArray() {
        $panelGroup = $('#quant-modal');
        var ret = [];
        var index = 0;
        $panelGroup.find('.panel').each(function () {
            var quants = {};
            $(this).find('.quant-factor-slider').each(function () {
                var $this = $(this);
                quants[$this.attr('id')] = $this.data('slider').getValue();
            });
            ret[index] = quants;
            index++;
        });
        return ret;
    }

    // retrieve the progressive quantization factor values
    function getProgQuantizationArray() {
        $panelGroup = $('#progquant-modal');
        var ret = [];
        var index = 0;
        $panelGroup.find('.tab-pane').each(function () {
            var layer = [];
            var component = 0;
            $(this).find('.panel').each(function () {
                var quants = {};
                $(this).find('.quant-factor-slider').each(function () {
                    var $this = $(this);
                    quants[$this.attr('id')] = $this.data('slider').getValue();
                });
                layer[component] = quants;
                component++;
            });
            ret[index] = layer;
            index++;
        });
        return ret;
    }

    // retrieve the UseReduceExtrapolate value
    function getUseReduceExtrapolate(query) {
        $panelGroup = $(query).parents('.panel-group');
        return {
            Enabled: $panelGroup.find('.use-reduce-extrapolate').prop("checked")
        }
    }

    // retrieve the UseHexInput value
    function getFirstInputBoxInputFormat(query) {
        $panelGroup = $(query).parents('.panel-group');
        return $panelGroup.find('.dec-hex-selector').val()
    }

    // retrieve the DataFormatInput value
    function getDataFormatInput(query) {
        $tabPane = $(query).parents('.tab-pane');

        // Hex or Dec
        $format = $tabPane.find('.dec-hex-selector');
        if ($format.length != 0) return $format.val();

        // Integer or Fixed Point Integer
        $format = $tabPane.find('.input-data-format');
        if ($format.length == 0) return "Integer";
        else return $format.val();
    }

    // retrieve the UseDifferenceTile value
    function getUseDifferenceTile(query) {
        $panelBody = $(query).parents('.panel-body');
        return {
            Enabled: $panelBody.find('input[name="use-diffing"]').val()
        }
    }

    // retrieve the RFXPEncodeUse DifferenceTile value
    function getRFXPDecodeUseDiffing(query) {
        $panelGroup = $(query).parents('.panel-group');
        return {
            Enabled: $panelGroup.find(".use-difference-tile").prop("checked")
        }
    }

    // get the current active layer
    function getLayer() {
        return $('#rfxpdecode-layer-tabs li.active').index();
    }

    // insert a span with text color
    function insertSpan(text, color) {
        return '<span style="color:' + color + '">' + text + '</span>';
    }

    // pad a string to a fixed width
    String.prototype.paddingLeft = function (width) {
        if (this.length > width) {
            return this.substr(0, width);
        } else {
            var padding = "";
            for (var c = this.length; c < width; c++) padding += " ";
            return padding + this;
        }
    }

    $('#remotefx-encoding-codec').click(
        function () {
            var mockFile = { name: "LoadExample", size: 12345 };
            var ImageDropZone = Dropzone.forElement("#dropUploader");
            ImageDropZone.options.addedfile.call(ImageDropZone, mockFile);
            ImageDropZone.options.thumbnail.call(ImageDropZone, mockFile, location.pathname + "/../static/RFXEncodeExample.bmp");
            ImageDropZone.files.push(mockFile);
            $.ajax({
                type: 'GET',
                url: location.pathname + "/SetSamples?sample=RFXEncodeExample.bmp"
            });
        }
        );

    $('#remotefx-decoding-codec').click(
        function () {
            $.ajax({
                type: 'GET',
                url: location.pathname + "/../static/RFXDecodeSample.txt",
            }).done(function (data) {
                var encodedData = JSON.parse(data);
                $('#encoded-input-y textarea').text(arrayToString(encodedData.YData));
                $('#encoded-input-cb textarea').text(arrayToString(encodedData.CbData));
                $('#encoded-input-cr textarea').text(arrayToString(encodedData.CrData));
                $('#encoded-input .entropy-alg').val("RLGR3");
                $('#encoded-input .dec-hex-selector').val("hex");
            })
        }
        );

    function arrayToString(array) {
        var str = "";
        var count = 0;
        array.forEach(
            function (value) {
                count++;
                str += value + (count % 16 == 0 ? "\n" : " ");
            }
            );
        return str;
    }

    $('.data-format-selector').change(ChangeDataFormat);

    function ChangeDataFormat() {
        var $this = $(this);
        var value = $(this).find('option:selected').val();
        var contents = $(this).closest('.tab-pane').find('.content .tabs .tab-content .tab-data-content .well');
        var currentType = $(this).siblings(':input[name="dataformat"]');
        var curType = currentType.val();

        contents.each(function () {
            var content = $(this).text();
            if (content == "") return;
            if (content == "No data available.") return;
            var array = content.match(/\S+/g);
            switch (curType) {
                case "Integer":
                    if (value == "11.5") {
                        for (var i = 0; i < array.length; i++) {
                            array[i] = array[i] << 5;
                        }
                    }
                    if (value == "12.4") {
                        for (var i = 0; i < array.length; i++) {
                            array[i] = array[i] << 4;
                        }
                    }
                    break;
                case "11.5":
                    if (value == "Integer") {
                        for (var i = 0; i < array.length; i++) {
                            array[i] = array[i] >> 5;
                        }
                    }
                    if (value == "12.4") {
                        for (var i = 0; i < array.length; i++) {
                            array[i] = array[i] >> 1;
                        }
                    }
                    break;
                case "12.4":
                    if (value == "11.5") {
                        for (var i = 0; i < array.length; i++) {
                            array[i] = array[i] << 1;
                        }
                    }
                    if (value == "Integer") {
                        for (var i = 0; i < array.length; i++) {
                            array[i] = array[i] >> 4;
                        }
                    }
                    break;
                default:
            }
            var text = FormatOutputString(array);
            $(this).text(text);
        }
        );

        currentType.val(value);
    }

    $('.dec-hex-selector').change(ChangeDecOrHex);

    function ChangeDecOrHex() {
        var $this = $(this);
        var value = $(this).find('option:selected').val();
        var contents = $(this).closest('.tab-pane').find('.content .tabs .tab-content .tab-data-content .well');

        contents.each(function () {
            var content = $(this).text();
            if (content == "") return;
            if (content == "No data available.") return;
            var array = content.match(/\S+/g);
            if (value == "dec") {
                for (var i = 0; i < array.length; i++) {
                    array[i] = parseInt(array[i], 16);
                }
            } else {
                for (var i = 0; i < array.length; i++) {
                    var addZero = array[i] < 0x10;
                    array[i] = Number(array[i]).toString(16).toUpperCase();
                    if (addZero) array[i] = '0' + array[i];
                }
            }
            
            var text = FormatOutputString(array);
            $(this).text(text);
        }
        );
    }

    function FormatOutputString(array) {
        var tileSize = 64;
        var cols = 12;
        if (array.length == tileSize * tileSize) {
            cols = tileSize;
        }
        var text = "";
        for (var i = 0; i < array.length; i++) {
            text += array[i] + ((i+1) % cols == 0 ? "\n" : "\t");
        }
        return text;
    }

    // maintain a reference to the current panel object
    var _currentpanel = null;

    // when a functional stage panel is clicked
    // send ajax request to get the data in it
    $('.panel-heading .panel-title a').click(PanelClick);

    function PanelClick() {
        var $this = $(this);

        // if the click is collapsing the panel
        // do not make the ajax request
        if ($this.parents('.panel')
                 .find('.panel-collapse')
                 .hasClass('in')) return;

        // update the _currentPanel object on each click
        _currentpanel = $this.parents('.panel');

        // get the panel title -- the action name
        var name = $this.text().trim();

        // special case for the Previous Frame
        if ($(this).text() == "Previous Frame") {
            $.ajax({
                type: 'POST',
                url: location.pathname + '/PreviousFrame',
                contentType: "application/json",
            }).done(function (data) {
                if (data) {
                    var mockFile = { name: "ValidPreviousFrame", size: 12345 };
                    var preFrameDropZone = Dropzone.forElement("#previousFrameUploader");
                    preFrameDropZone.options.addedfile.call(preFrameDropZone, mockFile);
                    preFrameDropZone.options.thumbnail.call(preFrameDropZone, mockFile, data);
                }
            }
            );
        }

        // get the current layer
        var layer = getLayer();
        // format the request url
        // var url = (location.pathname === '/') ?
        //    "/Panel" : location.pathname + "/Panel";
        var data = { name: name };
        if (layer >= 0) {
            data = { name: name, layer: layer };
        }

        // ajax request to get content for output-tab-pane
        $.ajax({
            type: 'POST',
            url: location.pathname + '/LayerPanel',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function (data) {
            if (data) {
                var $content = _currentpanel.find('.content.output-tab-pane');
                var layers = $(data).find('input[name="has-layers"]').val();
                if (layers > 0) {
                    // update the content
                    $content.html(data);
                } else {
                    $content.append(data);
                }
                UpdatePanel(name, $content);
            }
        });

        // ajax request to get content for input-tab-pane
        $.ajax({
            type: 'POST',
            url: location.pathname + '/InputPanel',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function (data) {
            if (data) {
                // update the content
                var $content = _currentpanel.find('.content.input-tab-pane');
                var layers = $(data).find('input[name="has-layers"]').val();
                if (layers > 0) {
                    // update the content
                    $content.html(data);
                } else {
                    $content.append(data);
                }
            }
        });
    }

    function UpdatePanel(panelName, content) {
        if (panelName == 'Sub-Band Diffing') {
            var notused = '<span class="glyphicon glyphicon-eye-close" style="padding-right: 5px;"></span>Diffing Not Used.';
            var used = '<span class="glyphicon glyphicon-eye-open" style="padding-right: 5px;"></span>Diffing Used.';
            var diffing = $(content).find('input[name="use-diffing"]').val();
            if (diffing == 'True') {
                $(content).siblings('.use-diffing').html(used);
            } else {
                $(content).siblings('.use-diffing').html(notused);
            }
        }
    }

    // add more layers
    $('#rfxpdecode-layer-add').click(function () {
        var index = $('#rfxpdecode-layer-tabs li').length - 1;
        var liTag = '<li><a href="#tab-layer' + index + '" data-toggle="tab">Layer ' + index + '</a></li>';
        $('#rfxpdecode-layer-tabs li:last').before(liTag);

        // TODO: directly open the lastest tab
        //$('#rfxpdecode-layer-tabs:nth-last-child(2)').click();

        // disable decode button in previous tab
        $('#rfxpdecode-layer' + (index - 1)).attr("disabled", "disabled");

        // update the decode status
        $.ajax({
            type: 'GET',
            url: location.pathname + '/UpdateDecodeStatus?layer=' + (index - 1),
        });

        // request new codec panels
        $.ajax({
            type: 'GET',
            url: location.pathname + '/CodecTab?id=' + index,
        }).done(function (data) {
            // if data is empty, just do nothing
            if (data) {
                $('#tab-layers').append(data);

                // Add click event handler to new decode button
                $('#rfxpdecode-layer' + index).click(RfxPDecode);
                // Initialize sliders
                var layerTag = "#tab-layer" + index;
                $(layerTag + ' .quant-popover').modalPopover({
                    target: $(layerTag + ' .quant-setting'),
                    placement: 'top'
                });
                // show popover on click
                $(layerTag + ' .quant-setting').click(function () {
                    $(layerTag + ' .quant-popover').modalPopover('toggle');
                });

                // make quantization factors into sliders
                $(layerTag + ' .quant-factor-slider').slider();
                $(layerTag + ' .slider-horizontal').width('100%');

                // update the copy button
                client = new ZeroClipboard($('.btn-copy'));
                AddClipboardListener();

                // add panel click handler
                $(layerTag + ' .panel-heading .panel-title a').click(PanelClick);

                // source code button
                $('.btn-source-code').each(function () {
                    $(this).click(function () {
                        var url = $(this).siblings('input[name="srcUrl"]').val();
                        var currentURL = location.href;
                        window.open(currentURL + "/../" + url, '_blank');
                    });
                });

                // open the tile input tab
                $(layerTag + ' .panel-group .panel-heading:first').find('a').click();
            }
        });
    });

    // Remove one layer of quantizations
    $('#progquant-layer-remove').click(function () {
        var count = $('#progquant-layer-remove').parent('ul').find('li').length;
        $('#progquant-layer-remove').parent('ul').find('li:nth-last-child(3)').remove();
        $('#progquant-layer-tabs-contents .tab-pane:last').remove();
        $('#progquant-layer-remove').parent('ul').find('li:nth-last-child(3) a').click();
    });

    // Remove one layer of quantizations
    $('#progquant-layer-add').click(function () {
        // Request zero progressive quantizations
        $.ajax({
            type: 'GET',
            url: location.pathname + '/DefaultProgQuants',
        }).done(function (data) {
            // if data is empty, just do nothing
            if (data) {
                var count = $('#progquant-layer-tabs-contents .tab-pane').length;
                var liStr = '<li class=""><a href="#quant-tab-layer-' + count + '" data-toggle="tab">Layer ' + count + '</a></li>';
                $('#progquant-layer-remove').parent('ul').find('li:nth-last-child(2)').before(liStr);

                var panelStr = '<div class="tab-pane" id="quant-tab-layer-' + count + '">';
                panelStr = panelStr.concat(data);
                panelStr = panelStr.concat('</div>');
                $('#progquant-layer-tabs-contents').append(panelStr);

                // make quantization factors into sliders
                $('.quant-factor-slider').slider();
                $('.slider-horizontal').width('100%');

                $('#progquant-layer-remove').parent('ul').find('li:nth-last-child(3) a').click();
            }
        });
    });

    // response to compare button
    $('.btn-compare').each(function () {
        $(this).click(function () {
            // if the compare is clicked, 
            // update the current panel reference
            _currentpanel = $(this).parents('.panel');

            var $modal = $('.diff-modal'),
                $prompt = $modal.find('.diff-prompt'),
                $input = $modal.find('.diff-input'),
                $result = $modal.find('.diff-result'),
                $status = $modal.find('.diff-status'),
                $button = $modal.find('.dlg-btn-compare');

            // update prompt message
            var currentPanel = _currentpanel.find('.panel-title').text(),
                currentTab = _currentpanel.find('.content.output-tab-pane .nav-tabs li.active').text();

            $prompt.text('Paste your data of Dimension ' + currentTab +
                         ' from the output of ' + currentPanel +
                         ' here: ');

            // clear the input box and result box
            $input.val("");
            $result.text("");
            $status.html("");
            $input.show();
            $result.hide();
            $button.text("Compare");

            $modal.modal('show');
        });
    });

    // reponse to the ok button in a compare dialog
    $('.dlg-btn-compare').click(function () {
        var $panel = _currentpanel.find('.panel-collapse.in'),
            $tab = $panel.find('.content.output-tab-pane .tab-pane.active.tab-data-content'),
            $modal = $('.diff-modal'),
            $input = $modal.find('.diff-input'),
            $result = $modal.find('.diff-result'),
            $status = $modal.find('.diff-status');
        // for the multi-layer data
        if ($panel.find('.content.output-tab-pane .tab-pane.layer-panes.active').length != 0) {
            $tab = $panel.find('.content.output-tab-pane .tab-pane.layer-panes.active .tab-pane.active.tab-data-content')
        }

        if ($(this).text() == 'Compare') {
            $(this).text('Return');
            // the array data in the tab content, splited by whitespace
            var tabData = $tab.text().trim().split(/\s+/),
                // the array user pasted, splited by whitespace
                userData = $modal.find('textarea').val().trim().split(/\s+/);

            var diffResult = diff(tabData, userData);

            var status = "";
            if (tabData.length != userData.length) {
                status = '<font color="blue">The length of input is different with the output.</font>';
            } else {
                status = '<font color="blue">The Mean Squared Error is <b>' + mse(tabData, userData) + '</b>.' +
                    ' When MSE is zero two data are exactly the same. The larger MSE value is the larger difference is.</font>';
            }

            // hide the input box and show the result box
            $input.hide();
            $status.html(status);
            $result.html(diffResult).show();
        } else {
            $(this).text('Compare');
            $input.val('');
            $result.html('');
            $status.html('');
            // hide the result box and show the input box
            $result.hide();
            $input.show();
        }
    });

    function diff(tabData, userData) {
        // local variables
        var cols = 12,
            diffResult = "";

        // Display a matrix if it is a matrix
        if (tabData.length == 64 * 64) {
            cols = 64;
        }

        var length = userData.length < tabData.length
                   ? userData.length
                   : tabData.length;

        diffResult += '<table class="diff-table">';

        var tableBody = '<tbody>';

        // column number
        tableBody += '<tr class="column-number">';
        tableBody += '<td class="line-number" scope="row"></td>';
        var colNum = "";
        for (var i = 1; i <= cols; i++) {
            colNum += spaces(i) + i;
        }
        tableBody += '<td class="col-number">' + colNum + '</td>';
        tableBody += '</tr>';

        var lineCount = 0;
        for (var row = 0; row < Math.ceil(length / cols) ; row++) {
            var lineDiff = 0,
                originalLine = "",
                diffLine = "";

            for (var i = 0; i < cols; i++) {
                var index = row * cols + i;
                if (index < length) {
                    if (tabData[index] != userData[index]) {
                        lineDiff++;
                        originalLine += spaces(tabData[index]) + '<span class="different-element">' + tabData[index] + '</span>';
                        diffLine += spaces(userData[index]) + '<span class="different-element">' + userData[index] + '</span>';
                    } else {
                        originalLine += spaces(tabData[index]) + tabData[index];
                        diffLine += spaces(userData[index]) + userData[index];
                    }
                }
            }

            if (lineDiff > 0) {
                // original
                if (lineDiff == cols) {
                    tableBody += '<tr class="different-original-line complete-different">';
                } else {
                    tableBody += '<tr class="different-original-line">';
                }
                tableBody += '<td class="line-number" scope="row"></td>';
                tableBody += '<td class="diff-content">' + originalLine + '</td>'
                tableBody += '</tr>';
                lineCount++;

                // diff
                if (lineDiff == cols) {
                    tableBody += '<tr class="different-user-line complete-different">';
                } else {
                    tableBody += '<tr class="different-user-line">';
                }
                tableBody += '<td class="line-number" scope="row">' + (row + 1) + '</td>';
                tableBody += '<td class="diff-content">' + diffLine + '</td>'
                tableBody += '</tr>';
                lineCount++;
            } else {
                tableBody += '<tr class="original-line">';
                tableBody += '<td class="line-number" scope="row">' + (row + 1) + '</td>';
                tableBody += '<td class="diff-content">' + originalLine + '</td>'
                tableBody += '</tr>';
                lineCount++;
            }
        }
        diffResult += tableBody + '</table>';

        return diffResult;
    }

    function mse(tabData, userData) {
        if (tabData.length != userData.length) {
            return 'Different Length';
        } else {
            var sum = 0.0;
            for (var i = 0; i < tabData.length; i++) {
                sum += (tabData[i] - userData[i]) * (tabData[i] - userData[i]);
            }
            return sum / tabData.length;
        }
    }

    function spaces(data) {
        var formated = "";
        var length = 5 - data.toString().length;
        for (var i = 0; i < length; i++) {
            formated = formated + "&nbsp;";
        }
        return formated;
    }

    $('.btn-source-code').each(function () {
        $(this).click(function () {
            var url = $(this).siblings('input[name="srcUrl"]').val();
            var currentURL = location.href;
            window.open(currentURL + "/../" + url, '_blank');
        });
    });

    // response to clear button
    $('.btn-clear').each(function () {
        $(this).click(function () {
            $(this).parents('.tab-pane').find('.tab-pane.active textarea').val("");
        });
    });

    // response to recompute button for rfx-encode-decode
    $('.btn-recompute').each(function () {
        $(this).click(function () {
            var button = this;
            // Action name
            var action = $(this).parents('.panel').find('.panel-heading .panel-title a').text().trim();
            // Retrive inputs in an array
            var layer = 0;
            var inputs = [];
            $(this).parents('.tab-pane:eq(0)').find('.tabs').each(function () {
                var tile = [];
                var index = 0;
                $(this).find('textarea').each(function () {
                    tile[index++] = $(this).val();
                });
                inputs[layer++] = tile;
            });

            var data = {
                Action: action,
                // wrap all parameters
                Params: {
                    // Only meaning full parameters will be used.
                    // but all the parameters are sent.
                    EntropyAlgorithm: getEntropyAlgorithm(this),
                    QuantizationFactorsArray: getQuantizationArray(),
                    ProgQuantizationArray: getProgQuantizationArray(),
                    UseReduceExtrapolate: getUseReduceExtrapolate(this),
                    UseDifferenceTile: getRFXPDecodeUseDiffing(this),
                    UseDataFormat: getDataFormatInput(this)
                },
                // wrap all inputs
                Inputs: inputs,
                Layer: getLayer()
            };

            // make the request in ajax
            $.ajax({
                type: 'POST',
                url: location.pathname + '/Recompute',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify(data),
            }).done(function () {
                RefreshOutputTabs(button);
                // Open output tab
                $(button).parents('.tabs').find('.nav.nav-tabs.nav-justified li:last a').click();
                // close all following panels
                $(button).parents('.panel').find('~.panel').each(function () {
                    if ($(this).find('.panel-collapse').hasClass('in')) {
                        $(this).find('.panel-heading .panel-title a').click();
                    }
                });
            });
        });
    });

    // refresh the output tabs in panel which contains element
    function RefreshOutputTabs(element) {
        // update the _currentPanel object on each click
        _currentpanel = $(element).parents('.panel');

        // get the panel title -- the action name
        var name = $(_currentpanel).find('.panel-heading .panel-title a').text().trim();
        // get the current layer
        var layer = getLayer();
        // format the request url
        var data = { name: name };
        if (layer >= 0) {
            data = { name: name, layer: layer };
        }

        // ajax request to get content for output-tab-pane
        $.ajax({
            type: 'POST',
            url: location.pathname + '/LayerPanel',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function (data) {
            if (data) {
                // update the content
                var $content = _currentpanel.find('.content.output-tab-pane');
                $content.html(data);
            }
        });
    }

    // response to input button
    $('.btn-export').each(function () {
        $(this).click(function () {
            var $modal = $('.output-as-input-modal');
            $modal.modal('show');
        });
    });

    // use output of RFXEncode as the input of RFXDecode
    $('.dlg-btn-output-as-input').click(function () {
        var tile = [];
        var index = 0;
        $(this).parents('.tab-pane').find('.tabs .well').each(function () {
            tile[index++] = $(this).text();
        });

        var data = {
            Params: {
                EntropyAlgorithm: getEntropyAlgorithm(this),
                QuantizationFactorsArray: getQuantizationArray()
            },
            // wrap all inputs
            Inputs: tile
        };

        // make the request in ajax
        $.ajax({
            type: 'POST',
            url: location.pathname + '/../RFXDecode/IndexWithInputs',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function () {
            // redirect to RFXDecode page
            window.location.href = location.pathname + '/../RFXDecode';
        });
    });

    // response to input button
    $('.btn-prog-export').each(function () {
        $(this).click(function () {
            var $modal = $('.prog-output-as-input-modal');
            var layerCount = $('#entropy-encoding-out .nav-layer ul li').length;
            if (layerCount == 0) {
                $modal.find('.modal-body h4').text("There are no results currently. Please retry after computing the results.");
            } else {
                $modal.find('.modal-body h4').text("How many layers of output will you use as the input of RFX Progressive Decoder?");
            }
            var radioString = '';
            for (var layer = 0; layer < layerCount; layer++) {
                radioString += '<label class="radio">';
                if (layer == 0) {
                    radioString += '<input type="radio" name="select-layer" id="prog-out-as-in-layer" value="' + layer + '" checked>';
                } else {
                    radioString += '<input type="radio" name="select-layer" id="prog-out-as-in-layer" value="' + layer + '">';
                }
                if (layer == 0) {
                    radioString += 'Layer ' + layer;
                    radioString += '</label>';
                } else {
                    radioString += 'Layer 0 ~ Layer ' + layer;
                    radioString += '</label>';
                }
            }
            $modal.find('.prog-layer-radio').html(radioString);
            $modal.modal('show');
        });
    });

    $('.dlg-btn-prog-out-as-in').click(function () {
        var endlayer = $(this).parents('.modal-content').find('.prog-layer-radio :radio:checked').val();
        if (endlayer == undefined) {
            $('.prog-output-as-input-modal').modal('hide');
            return;
        }
        var inputs = [];
        var layer = 0;
        $(this).parents('.panel').find('#entropy-encoding-out .layer-panes').each(function () {
            var tile = [];
            var index = 0;
            $(this).find('.tab-pane .well').each(function () {
                tile[index++] = $(this).text();
            });
            inputs[layer++] = tile;
        });

        var params = {
            EntropyAlgorithm: getEntropyAlgorithm(this),
            QuantizationFactorsArray: getQuantizationArray(),
            ProgQuantizationArray: getProgQuantizationArray(),
            UseReduceExtrapolate: getUseReduceExtrapolate(this),
            UseDifferenceTile: getUseDifferenceTile(this),
            DecOrHex: getDataFormatInput(this)
        };

        var data = [];

        var layer = 0;
        inputs.forEach(function (input) {
            if (layer > endlayer) return;
            data[layer] = {
                Params: params,
                Inputs: input,
                Layer: layer
            };
            layer++;
        });

        // make the request in ajax
        $.ajax({
            type: 'POST',
            url: location.pathname + '/../RFXPDecode/IndexWithInputs',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function () {
            // redirect to RFXDecode page
            window.location.href = location.pathname + '/../RFXPDecode';
        });
    });

    $('#rfxdecode').click(function () {
        var data = {
            // wrap all parameters
            Params: {
                EntropyAlgorithm: getEntropyAlgorithm(this),
                QuantizationFactorsArray: getQuantizationArray(),
                DecOrHex: getFirstInputBoxInputFormat(this)
            },
            // wrap all inputs
            Inputs: {
                Y: $('#encoded-input-y textarea').val(),
                Cb: $('#encoded-input-cb textarea').val(),
                Cr: $('#encoded-input-cr textarea').val()
            }
        };
        // make the request in ajax
        $.ajax({
            type: 'POST',
            url: location.pathname + '/Decode',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function () {
            // set the default data format to Integer
            $('.data-format-selector').val("Integer");
            $('.data-format-previous').val("Integer");
            $('.input-data-format').val("Integer");
            $('.tab-pane .dec-hex-selector').val('dec');

            // hide the first panel and expend the second panel 
            $('.panel-group .panel-heading:first').find('a').click();
            $('.panel-group .panel-heading:eq(1)').find('a').click();
        });
    });

    $('#rfxencode').click(function () {
        var data = {
            // wrap all parameters
            Params: {
                EntropyAlgorithm: getEntropyAlgorithm(this),
                QuantizationFactorsArray: getQuantizationArray()
            }
        };
        // make the request in ajax
        $.ajax({
            type: 'POST',
            url: location.pathname + '/Encode',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function () {
            // set the default data format to Integer
            $('.data-format-selector').val("Integer");
            $('.data-format-previous').val("Integer");
            $('.input-data-format').val("Integer");
            $('.tab-pane .dec-hex-selector').val('dec');

            // hide the first panel and expend the second panel 
            $('.panel-group .panel-heading:first').find('a').click();
            $('.panel-group .panel-heading:eq(1)').find('a').click();
        });
    });
    // TODO:
    // when dropzone fails uploading files, it produces terrible 
    // long error message. we should do something to deal with 
    // those ugly error message.


    $('#rfxpencode').click(function () {
        var data = {
            // wrap all parameters
            Params: {
                EntropyAlgorithm: getEntropyAlgorithm(this),
                QuantizationFactorsArray: getQuantizationArray(),
                ProgQuantizationArray: getProgQuantizationArray(),
                UseReduceExtrapolate: getUseReduceExtrapolate(this)
            }
        };
        // make the request in ajax
        $.ajax({
            type: 'POST',
            url: location.pathname + '/Encode',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function () {
            // set the default data format to Integer
            $('.data-format-selector').val("Integer");
            $('.data-format-previous').val("Integer");
            $('.input-data-format').val("Integer");
            $('.tab-pane .dec-hex-selector').val('dec');

            // hide the first panel and expend the second panel 
            $('.panel-group .panel-heading:first').find('a').click();
            $('.panel-group .panel-heading:eq(1)').find('a').click();
        });
    });

    $('#rfxpdecode-layer0').click(RfxPDecode);

    // RfxPDecode button click event handler
    function RfxPDecode() {
        var layer = getLayer();
        var Inputs;
        if (layer == 0) {
            Inputs = [
               $('#tile-input-layer0-y textarea').val(),
               $('#tile-input-layer0-cb textarea').val(),
               $('#tile-input-layer0-cr textarea').val()
            ];
        } else {
            Inputs = [
                $('#tile-input-layer' + layer + '-y textarea').val(),
                $('#tile-input-layer' + layer + '-y-raw-data textarea').val(),
                $('#tile-input-layer' + layer + '-cb textarea').val(),
                $('#tile-input-layer' + layer + '-cb-raw-data textarea').val(),
                $('#tile-input-layer' + layer + '-cr textarea').val(),
                $('#tile-input-layer' + layer + '-cr-raw-data textarea').val()
            ];
        }
        var data = {
            // wrap all parameters
            Params: {
                EntropyAlgorithm: getEntropyAlgorithm(this),
                QuantizationFactorsArray: getQuantizationArray(),
                ProgQuantizationArray: getProgQuantizationArray(),
                UseReduceExtrapolate: getUseReduceExtrapolate(this),
                UseDifferenceTile: getRFXPDecodeUseDiffing(this),
                DecOrHex: getFirstInputBoxInputFormat(this)
            },
            Inputs: Inputs,
            Layer: getLayer()
        };

        // make the request in ajax
        $.ajax({
            type: 'POST',
            url: location.pathname + '/Decode',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function () {
            // set the default data format to Integer
            $('.data-format-selector').val("Integer");
            $('.data-format-previous').val("Integer");
            $('.input-data-format').val("Integer");
            $('.tab-pane .dec-hex-selector').val('dec');

            // hide the first panel and expend the second panel 
            $('.tab-pane.active .panel-group .panel-heading:first').find('a').click();
            $('.tab-pane.active .panel-group .panel-heading:eq(1)').find('a').click();
        });
    }

    // mse psnr
    $('#ImageCompare').click(function () {
        var dropzone1 = Dropzone.forElement('#ImageCompareUploader1');
        var dropzone2 = Dropzone.forElement('#ImageCompareUploader2');
        if (dropzone1.getAcceptedFiles().length == 0 || dropzone2.getAcceptedFiles().length == 0) {
            var dangerAlert = GenerateAlert('danger'),
                content = '<strong>Error!</strong> Before comparing please input two images.',
                alertEnd = '</div>';

            $('.compare-status').html(dangerAlert + content + alertEnd);
            return;
        }

        $.ajax({
            type: 'POST',
            url: location.pathname + '/Compare',
            contentType: "application/json",
        }).done(function (data) {
            if (data['info'] == "Error") {
                var dangerAlert = GenerateAlert('danger'),
                    content = '<strong>Error!</strong> ' + data['value'],
                    alertEnd = '</div>';

                $('.compare-status').html(dangerAlert + content + alertEnd);

            } else if (data['info'] == 'Same') {
                var dangerAlert = GenerateAlert('success'),
                    content = data['value'],
                    alertEnd = '</div>';

                $('.compare-status').html(dangerAlert + content + alertEnd);

            } else {
                var dangerAlert = GenerateAlert('warning'),
                    content = data['value'],
                    alertEnd = '</div>';

                $('.compare-status').html(dangerAlert + content + alertEnd);
            }
        });
    });

    // ssim ms-ssim g-ssim
    $('#ssimImageCompare').click(function () {
        var dropzone1 = Dropzone.forElement('#ImageCompareUploader1');
        var dropzone2 = Dropzone.forElement('#ImageCompareUploader2');
        if (dropzone1.getAcceptedFiles().length == 0 || dropzone2.getAcceptedFiles().length == 0) {
            var dangerAlert = GenerateAlert('danger'),
                content = '<strong>Error!</strong> Before comparing please input two images.',
                alertEnd = '</div>';

            $('.compare-status').html(dangerAlert + content + alertEnd);
            return;
        }

        var param = {};
        $('.ssim-param').each(function () {
            param[$(this).attr('name')] = $(this).val();
        });

        $.ajax({
            type: 'POST',
            url: location.pathname + '/Compare',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(param),
        }).done(function (data) {
            if (data['info'] == "Error") {
                var dangerAlert = GenerateAlert('danger'),
                    content = '<strong>Error!</strong> ' + data['value'],
                    alertEnd = '</div>';

                $('.compare-status').html(dangerAlert + content + alertEnd);

            } else if (data['info'] == 'Same') {
                var dangerAlert = GenerateAlert('success'),
                    content = data['value'],
                    alertEnd = '</div>';

                $('.compare-status').html(dangerAlert + content + alertEnd);

            } else {
                var dangerAlert = GenerateAlert('warning'),
                    content = data['value'],
                    alertEnd = '</div>';

                $('.compare-status').html(dangerAlert + content + alertEnd);
            }
        });
    });

    function GenerateAlert(type) {
        return '<div class="alert alert-' + type + ' alert-dismissible" role="alert">' +
                    '<button type="button" class="close" data-dismiss="alert">' +
                      '<span aria-hidden="true">&times;</span>' +
                      '<span class="sr-only">Close</span>' +
                    '</button>';
    }
})
