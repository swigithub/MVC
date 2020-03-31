
      $(function () {
          var nodeTypeList;
          $(document).ready(function () {

              $.ajax({
                  url: "/Project/Template/SavedFormInfo/",
                  type: "post",
                  data: { formId: "-1" },
                  async: false,
              }).done(function (data) {

                  nodeTypeList = data.formTypeList;

                  $('#ddNodeTypeId').append("<option value='-1'> Select </option>");
                  for (var i = 0; i < nodeTypeList.length; i++) {
                      $('#ddNodeTypeId').append("<option value='" + nodeTypeList[i].DefinationId + "'> " + nodeTypeList[i].DefinationName + " </option>");
                  }
              });

          });

         

          $('#routeTemplateId').val(@Url.RequestContext.RouteData.Values["id"]);
          localStorage.removeItem("dataForGridStack");

          var globalID = 0;
          $("#add-node").draggable({
              helper: "clone"
          });
          $("#add-chart").draggable({
              helper: "clone"
          });

          $("#grid-stack-DIV").droppable({
              accept: "#add-node, #add-chart",

              drop: function (event, ui) {
                  switch (ui.draggable.attr('id')) {
                      case "add-node":
                          $('#settingsModal').modal('show');
                          break;
                      case "add-chart":
                          // chart logic
                          break;
                  }
              }
          });

          $('#add-node').click(function () {
              $('#settingsModal').modal('show');
          });

          $('#ddlTemplateName').change(function () {
              loadGrid();
          });

          $('#add-chart').click(function () {
              console.log('add-chart clicked');
          });

          $('#modalBtnOk').click(function () {
              var nodeHeight = $('#nodeHeight').val();
              var nodeWidth = $('#nodeWidth').val();
              var nodeType = $('#nodeType').val();
              var nodeTypeId = $('#ddNodeTypeId option:selected').val() + "-" + $('#ddNodeTypeId option:selected').text();
                
              var nodeId = $('#nodeId').val();
                
              grid = $('.grid-stack').data('gridstack');
              grid.addWidget($('<div> <div class="grid-stack-item-content">  <ul class="handler"> <li><a class="grippy"></a></li> <li><a class="remove"></a></li> <li> <div class="changeSetting" onclick="changeSettings(\'' + nodeId + '\')"> <a class="setting"></a> </div> </li> </ul> <div style="background-color:white;width: 100%;position:absolute;"> ' + nodeId + '</div> </div> </div>'), 0, 0, nodeWidth, nodeHeight, false, null, null, null, null, nodeTypeId);

          });

          var options = {
              animate: true,
              verticalMargin: 5,
              horizontalMargin: 5,

              draggable: {
                  handle: '.grippy',
              }
          };

          $('.grid-stack').gridstack(options);

          new function () {
              grid = $('.grid-stack').data('gridstack');

              loadGrid = function () {
                  localStorage.removeItem("dataForGridStack");
                  grid.removeAll();
                  var gridData = "";
                  serializedData = "";

                  var routeTemplateId = $('#routeTemplateId').val();

                  $.ajax({
                      url: "/Project/Template/GetNodeData/",
                      type: "GET",
                      data: { templateId: routeTemplateId },
                      async: false,
                  }).done(function (data) {
                      gridData = JSON.parse(JSON.stringify(data));

                      if (gridData != null && gridData != "") {
                          serializedData = gridData;
                      }
                      else {
                          serializedData = "[{ \"x\": \"0\", \"y\": \"5\", \"width\": \"8\", \"height\": \"4\", \"id\": \"19\" }]";
                      }

                      if (localStorage['dataForGridStack'] == null || localStorage['dataForGridStack'] == undefined) {
                          localStorage.setItem('dataForGridStack', JSON.parse(JSON.stringify(serializedData)));
                          serializedData = JSON.parse(localStorage['dataForGridStack']);
                      }
                      else {
                          serializedData = JSON.parse(localStorage['dataForGridStack']);
                      }

                  });


                  var items = GridStackUI.Utils.sort(serializedData);
                  //
                  _.each(items, function (node) {

                      //if (node.id != 0 && node.id != "") {

                      //    $.get("/Project/Template/GetWidget/" + node.id + "")
                      //        .done(function (data) {
                      //            grid = $('.grid-stack').data('gridstack');
                      //            grid.addWidget($('<div> <div class="grid-stack-item-content">  <ul class="handler" style="display:none;"> <li><a class="grippy"></a></li> <li><a class="remove"></a></li> <li> <div class="changeSetting" onclick="changeSettings(\'' + node.id + '\')"> <a class="setting"></a> </div> </li> </ul> <div style="background-color:white;width: 100%;position:absolute;"> ' + data + '</div> </div> </div>'), node.x, node.y, node.width, node.height, false, null, null, null, null, node.id);


                      //            if (localStorage['IsPublished'] == 'true') {
                      //                grid = $('.grid-stack').data('gridstack');
                      //                grid.enableMove(false);
                      //                grid.enableResize(false);
                      //                $('.handler').hide();
                      //            }
                      //        });
                      //}
                      //else {
                      grid.addWidget($('<div> <div class="grid-stack-item-content">  <ul class="handler" style="display:none;"> <li><a class="grippy"></a></li> <li><a class="remove"></a></li> <li> <div class="changeSetting" onclick="changeSettings(\'' + node.id + '\')"> <a class="setting"></a> </div> </li> </ul> <div style="background-color:white;width: 100%;position:absolute;"> ' + node.id + '</div> </div> </div>'), node.x, node.y, node.width, node.height, false, null, null, null, null, node.id);
                      //}

                  }, this);


                  if (localStorage['IsPublished'] == 'true') {
                      grid = $('.grid-stack').data('gridstack');
                      grid.enableMove(false);
                      grid.enableResize(false);
                      $('.handler').hide();
                  }
                  return false;
              }.bind(this);


              addNewNode = function () {
                  var idForNode = $('#nodeId').val();
                  if (idForNode != null) {

                      $.get("/Project/Project/Template/GetWidget/" + idForNode + "")
                        .done(function (data) {

                            grid = $('.grid-stack').data('gridstack');
                            grid.addWidget($('<div> <div class="grid-stack-item-content">  <ul class="handler" style="display:none;"> <li><span class="grippy"></span></li> <li><span class="remove"></span></li> <li> <div class="changeSetting" onclick="changeSettings(\'' + idForNode + '\')"> <span class="glyphicon glyphicon-asterisk"></span> </div> </li> </ul> <div style="background-color:white;width: 100%;position:absolute;"> ' + data + '</div> </div> </div>'), 0, 0, 1, 1, false, null, null, null, null, "" + idForNode + "");
                        });
                  }

              }.bind(this);

              saveGrid = function () {
                  serializedData = _.map($('.grid-stack > .grid-stack-item:visible'), function (el) {
                      el = $(el);
                      var node = el.data('_gridstack_node');
                        
                      debugger;
                      if (node != null) {
                          return {
                              id: node.id,
                              x: node.x,
                              y: node.y,
                              width: node.width,
                              height: node.height,
                          };
                      }
                  }, this);

                  pagePublished();

                  $.post("/Project/Template/SaveGrid", {
                      gridInfo: JSON.stringify(serializedData, null, '     '),
                      dashboardId: $('#routeTemplateId').val()
                  }).done(function (data) {

                  });


                  $('#saved-data').val(JSON.stringify(serializedData, null, '     '));
                  localStorage['dataForGridStack'] = JSON.stringify(serializedData, null, '       ');


              }.bind(this);


              clearGrid = function () {
                  grid.removeAll();
                  return false;
              }.bind(this);

              remove_grid = function () {
                  var grid = $('.grid-stack').data('gridstack'),
                  el = $(this).closest('.grid-stack-item')
                  grid.removeWidget(el);
              }.bind(this);


              clearCache = function () {
                  localStorage.removeItem("dataForGridStack");
              }

              pagePublished = function () {
                  localStorage['IsPublished'] = true;
                  if (localStorage['IsPublished'] == 'true') {
                      grid = $('.grid-stack').data('gridstack');
                      grid.enableMove(false);
                      grid.enableResize(false);

                      $('.handler').hide();
                  }
              }


              $('#save-grid').click(saveGrid);
              $('#load-grid').click(loadGrid);
              $('#clear-grid').click(clearGrid);
              $('#add-node-grid').click(addNewNode);
              $('#clear-cache').click(clearCache);
              $('.fa-remove').click($(this).remove_grid);

              $('#edit-grid').click(function () {
                  localStorage['IsPublished'] = false;

                  if (localStorage['IsPublished'] == 'false') {
                      grid = $('.grid-stack').data('gridstack');
                      grid.enableMove(true);
                      grid.enableResize(true);

                      $('.handler').show();
                  }
              });
              loadGrid();



              $('body').on('click', '.remove', function (e) {
                  e.preventDefault();
                  var grid = $('.grid-stack').data('gridstack'),
                  el = $(this).closest('.grid-stack-item')
                  var node = el.data('_gridstack_node');

                  if (node.id != null && !node.id.includes('-')) {

                      $.post("/Project/Template/RemoveNodeData", {
                          nodeId: node.id,
                      }).done(function (data) {
                          localStorage.removeItem("dataForGridStack");
                          loadGrid();
                      });

                  }
                  grid.removeWidget(el);
              });


              $('#editingSettingsModalBtnOk').click(function (e) {

                  $.post("/Project/Template/AddNodeRelaventData", {
                      nodeId: $("#settingModalId").val(),
                      nodeType: $("#ddlNodeRenderType").val(),
                      query: $('#queryLogic').val()
                  }).done(function (data) {
                      loadGrid();
                  });
              });
          };
      });

function changeSettings(id) {
    if (id == null || id == '') {
        alert('please save grid structure first');
    }
    else {

        var tempId = $('#routeTemplateId').val();

        $.ajax({
            url: "/Project/Template/GetRenderedFormHTML/",
            type: "post",
            data: { nodeId: id, templateId: tempId },
            async: false,
        }).done(function (data) {
            debugger;
            $('#formToRender').html(data);

        });

        $('#settingModalId').val(id);
        $('#editingSettingsModal').modal('show');
    }
}

