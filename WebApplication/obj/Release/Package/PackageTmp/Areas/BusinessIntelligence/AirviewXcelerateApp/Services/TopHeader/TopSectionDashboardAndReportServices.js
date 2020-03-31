

airviewXcelerateApp.factory('TopSectionDashboardAndReportService', function ($http, $timeout, toastr) {

    var TopSectionDashboardAndReportServiceHandler = function () {

        //#region load private and public tempaltes
        handleLoadPrivateAndPublicTemplate = function (vm) {
            $http({
                method: 'POST',
                url: '/BusinessIntelligence/DashboardNReport/GetTemplate',
                data: { userID: userIDByX1, filter: "GetPriAndPubTmpByUserID" }
            }).then(function successCallback(response) {
                if (response.data.status)
                    vm.loadTemp = JSON.parse(response.data.data);

                // this callback will be called asynchronously
                // when the response is available
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });
        };
        //#endregion

        //#region get entity template information
        var getEntityTemplateInfo = function (vm, scope, compile, entityId) {

            $.ajax({
                type: "GET",
                url: '/Entity/EntityTemplate/GetByEntityTemplateId',
                data: { id: entityId },
                success: function (result) {

                    if (result['EntityCategories'])
                        result['EntityCategory'] = result['EntityCategories']
                    scope.$emit('displayEntityTemplateNEntityOnLayoutE', result);
                }
            });
        };
        //#endregion

        //#region get entity
        var handleGetEntity = function (vm, scope, compile, entityId) {

            $.ajax({
                type: "GET",
                url: '/Entity/Entity/GetByEntityId',
                data: { id: entityId },
                success: function (result) {
                    scope.$emit('displayEntityTemplateNEntityOnLayoutE', result);

                }
            });
        }
        //#endregion 

        //#region render predefined views

        var handleRenderPredefinedView = function (preSelector, url, actualParams) {
            try {
                preSelector.load(url, actualParams, function (responseTxt, statusTxt, jqXHR) {

                    if (statusTxt == "success") {
                        //console.log(responseTxt);
                        //console.log(statusTxt);
                        //console.log(jqXHR);
                    }
                    if (statusTxt == "error") {
                        //console.log(statusTxt);
                    }

                })
            } catch (ex) {
                console.log(ex);

            }
        };

        //#endregion

        //#region update the url

        var handleUpdateUrl = function (vm, scope, compile) {
            var getCurrentUrl = new URL(window.location.href);
            if (getCurrentUrl.search) {
                var getQueryString = getCurrentUrl.search;

                var searchParams = new URLSearchParams(getQueryString);
                if (searchParams) {
                    let modTpe = "";
                    if (vm.isModeType == 'edit')
                        modTpe = 'EDIT';
                    if (vm.isModeType == 'view')
                        modTpe = 'VIEW';
                    searchParams.set('viewMode', modTpe);
                }
               
                getCurrentUrl.search = searchParams.toString();
                lastUrl = getCurrentUrl.toString();
                //window.history.pushState({}, document.title, udatedUrl);
                setTimeout(() => { window.history.replaceState({}, document.title, lastUrl); }, 1000)
            }
        };

        //#endregion 

        //#region update the url

        var handleUpdateUrlWhenAddEntity = function (vm, scope, compile,data) {
            var getCurrentUrl = new URL(window.location.href);
            if (getCurrentUrl.search) {
                var getQueryString = getCurrentUrl.search;

                var searchParams = new URLSearchParams("moduleType=EMS&moduleId=4&renderId=163817&viewMode=EDIT");
                if (searchParams) {
                    let modTpe = "";
                    if (vm.isModeType == 'edit')
                        modTpe = 'EDIT';
                    if (vm.isModeType == 'view')
                        modTpe = 'VIEW';
                    searchParams.set('viewMode', modTpe);
                    searchParams.set('moduleId', moduleInformation.ModuleTypeId);
                    searchParams.set('renderId', data['EntityId']);
                }
              
                getCurrentUrl.search = searchParams.toString();
                lastUrl = getCurrentUrl.toString();
                //history.pushState({}, document.title, udatedUrl);
                setTimeout(() => { window.history.replaceState({}, document.title, lastUrl); }, 1000);
            }
        };

        //#endregion 

        //#region where clause
          var handleMakeWhereClauseForTablesNViews = function (vm) {
            var whereClause = "("
            for (var i = 0; i < vm.tableNViews.length; i++) {
                whereClause += "'" + vm.tableNViews[i].Text + "'" + ","

            }
            whereClause = whereClause.substr(0, whereClause.length - 1)
            whereClause += ")";
            return whereClause;

        }
        //#endregion

        //#region get remote database

          var handleGetRemoteDatabase = function (vm, scope, compile) {
              $.ajax({
                  type: "POST",
                  url: '/BusinessIntelligence/QueryBuilder/GetRemoteDatabase',
                  data: vm.remoteDataSourceinfo,

                  success: function (result) {
                      if (result.status) {
                          let getData = JSON.parse(result.data);
                          if (getData[0]['errorMsg']) {
                              toastr.error(getData[0]['errorMsg']);
                              //vm.remoteDatabaseDdl = [];
                              //vm.remoteDataSourceinfo['remoteDatabase'] = "";
                              

                          }
                          else {
                              //vm.remoteDatabaseDdl = getData;
                              getColumns(vm, scope, compile)

                          }




                      }
                      else {
                          vm.remoteDatabaseDdl = [];
                          vm.remoteDataSourceinfo['remoteDatabase'] = "";
                          toastr.error("The username or password incorrect");
                      }
                      $('.rmt.overlay').attr('style', 'display:none;');

                  }
              });
          }

        //#endregion

        //#region get columns

          var getColumns = function (vm, scope, compile) {
              $.ajax({
                  type: "POST",
                  url: '/BusinessIntelligence/QueryBuilder/GetRemoteColumnsOfTablesNViews',
                  data: vm.remoteDataSourceinfo,
                  success: function (result) {
                      let timeout = 0;
                      if (result.status) {
                          let getData = JSON.parse(result.data);

                         
                          let dataSourceNTablesNViewsNColumns = { dataSource: vm.remoteDataSourceinfo, viewsTablesNColumn: getData, widgetIndex: 0 };
                          //timeout = getData.length;
                          // handleGroupByViewsNTables(getData);

                          scope.$emit('postViewsNTablesNColumnsRPTE', dataSourceNTablesNViewsNColumns);





                      }
                   


                  }
              });
          }

        //#endregion

          var getDataSetsNameFromDataTemplates = function (vm,scope,compile,rt) {
              
              if (rt != 0) {
                  $.ajax({
                      url: "/Market/GetDataTemplates?id=" + rt,
                      type: "GET",
                      success: function (res) {
                          //
                          var DataTemplates = res;
                          vm.tableNViews = res;
                          if (vm.tableNViews.length > 0) {
                              vm.remoteDataSourceinfo['remoteDatabaseWhereClause'] = handleMakeWhereClauseForTablesNViews(vm);
                              handleGetRemoteDatabase(vm, scope, compile)
                             
                          }
                      },
                      error: function (e) {
                          console.log("Error:" + e);
                      },
                      complete: function (msg) {

                      }
                  });
              }
          }


        var hanldeInitialization = function (vm, scope, compile, uibModal) {
            vm.coreModeTypeInte = 'EDIT';
            vm.isModeType = 'edit';
            vm.showpopover = false;
            vm.tableNViews = [];
            vm.remoteDataSourceinfo = {};
            vm.remoteDataSourceinfo['dataSourceName'] = remoteDataSourceinfo['dataSourceName'];
            vm.remoteDataSourceinfo['remoteUserName'] = remoteDataSourceinfo['remoteUserName'];
            vm.remoteDataSourceinfo['remotePassword'] = remoteDataSourceinfo['remotePassword'];
            vm.remoteDataSourceinfo['remoteDatabase'] = remoteDataSourceinfo['remoteDatabase'];
            vm.remoteDataSourceinfo['remoteProductName'] = "Sql Server";
            vm.remoteDataSourceinfo['remoteDatabaseWhereClause'] = "";
            scope.$on("changeModeTypeInTopHeaderB", function (event, modeTypefromdb) {
                vm.isModeType = modeTypefromdb;

            });
            scope.$on("refreshDropdownListB", function (event) {
                handleLoadPrivateAndPublicTemplate(vm);
            });

            scope.$on('InOtherModuleTempNotaddB', function (event, OtherModulesParams) {         
                vm.addTemStatus = OtherModulesParams.addTempStatus;
                vm.showpopover == OtherModulesParams.addTempStatus;
                scope.$apply();
            });
            scope.$on("setDefaultTemplateFromDashboardReportB", function (event, setDefaultTemplate) {
                if (setDefaultTemplate) { vm.classSetDefault = 'green'; }
                else { vm.classSetDefault = ""; }
            });
            scope.$on("setPublicTemplateFromDashboardReportB", function (event, setDefaultTemplate) {

                if (setDefaultTemplate) {
                    vm.classSetPublic = "green";
                }
                else { vm.classSetPublic = ""; }
            });

           
            if (moduleInformation.ModuleTypeId != null && moduleInformation.ModuleTypeId > 0) {
                getDataSetsNameFromDataTemplates(vm, scope, compile, moduleInformation.ModuleTypeId);
                vm.rt = moduleInformation.ModuleTypeId;
                if (moduleInformation.ViewMode == "VIEW") {
                    vm.coreModeTypeInte = 'VIEW1';
                    vm.isModeType = 'view';
                }
                else if (moduleInformation.ViewMode == "EDIT") {
                    vm.coreModeTypeInte = 'EDIT';
                    vm.isModeType = 'edit';
                }
                else {
                    vm.coreModeTypeInte = 'VIEW';
                }
                if (moduleInformation.ViewMode == "VIEW")
                    vm.isModeType = 'view';
                else
                    vm.isModeType = 'edit';
            }
            else if (moduleInformation.ModuleType != null) {
                vm.isModeType = edit = 'edit';
                vm.coreModeTypeInte = 'EDIT';
                vm.showpopover = true;
            }
            if (moduleInformation.ModuleTypeId) {
                //handleRenderPredefinedView($('body #entityPopArea'), '/Entity/Entity/LoadEntityModalPartial', {
                //    ModuleId: 8,
                //    FeatureId: 1,
                //    SubFeatureId: 1,
                //    mode: 'create',
                //    entityid: 0,
                //    consumerid: moduleInformation.ModuleTypeId
                //});
                //scope.$emit('enableTopHeaderE', true);
                //if (!moduleInformation.RenderId)
                //    getEntityTemplateInfo(vm, scope, compile, moduleInformation.ModuleTypeId)
            }
            //if (moduleInformation.RenderId) {
            //    handleGetEntity(vm, scope, compile, moduleInformation.RenderId);
            //}

            scope.addEntity = function (entityInformation) {
                scope.$emit('displayEntityTemplateNEntityOnLayoutE', entityInformation.result);
                scope.$emit('bindEntityTemplateIdOnAllWidgetE', entityInformation.result);
                handleUpdateUrlWhenAddEntity(vm, scope, compile, entityInformation.result);
                //console.log()
               

            }
            $('body, div').click(function () {
                if (lastUrl!="")
                    setTimeout(() => { window.history.replaceState({}, document.title, lastUrl); }, 1001);

            });
            // getEntityTemplateInfo(vm);
            //#region temp
            vm.classSetDefault = "";
            vm.classSetPublic = "";
            vm.open = false;
            vm.btnText = "Add New";
            // vm.isModeType = "view";
          //  vm.isModeType = "";
            vm.loadTemp = [];
            vm.addTemStatus = true;
           
            if (!moduleInformation.ModuleType) {
                handleLoadPrivateAndPublicTemplate(vm);
                vm.showpopover = true;
            }
            vm.shutterUpOrDown = shutterType.down;
            vm.changeShutterUpOrDown = function (event) {
                if (vm.shutterUpOrDown == 'shtrDown') {
                    vm.shutterUpOrDown = shutterType.up;
                    scope.$emit('changeShutterUpDownStatusE', vm.shutterUpOrDown);
                }
                else {
                    vm.shutterUpOrDown = shutterType.down;
                    scope.$emit('changeShutterUpDownStatusE', vm.shutterUpOrDown);
                }

            }
            vm.addNewEntity = function () {
                $('#new_entity').modal('show');

            }
            //$http({
            //    method: 'POST',
            //    url: '/BusinessIntelligence/DashboardNReport/GetTemplate',
            //    data: { userID: 6, filter: "GetAllPriTmpByUserID" }
            //}).then(function successCallback(response) {

            //    console.log(JSON.parse(response.data.data));
            //    // this callback will be called asynchronously
            //    // when the response is available
            //}, function errorCallback(response) {
            //    // called asynchronously if an error occurs
            //    // or server returns response with an error status.
            //});
            vm.loadTempFromDb = function () {
                scope.$emit('LoadForTempE', vm.selectTemp);
                vm.isModeType = "edit";
                vm.classSetDefault = "";
                vm.classSetPublic = "";
                isModeForTab = true;


            };
            //#endregion
            vm.saveTemplate = function () {
                scope.$emit('saveTemplateE');
            };
            vm.addNewTemplate = function () {

                vm.openmodel = uibModal.open({
                    templateUrl: 'dsrpt/DashboardNReport/getPartialPage/DashboardAndReport/_createDashboadAndReport.cshtml',
                    backdrop: 'static',
                    controller: "TopSectionModalInstance as vm",
                    resolve: {
                        parentScope: function () {

                            return scope;
                        },
                        PVm: function () {
                            return vm;
                        }
                    }
                });
                vm.openmodel.result.then(function () {

                });
            };
            vm.viewToEditMode = function () {
                scope.$emit('changeModeTypeE', 'edit');
                vm.isModeType = "edit";
                vm.coreModeTypeInte = "EDIT";
                handleUnlockGridStack();
                $('.new-template').removeClass('viewmode');
                isModeForTab = true;
                handleUpdateUrl(vm, scope, compile);

            };
            vm.editToViewMode = function () {
                vm.isModeType = "view";
                vm.coreModeTypeInte = "VIEW";
                scope.$emit('changeModeTypeE', 'view');
               $('.new-template').addClass('viewmode');
                handleLockGridStack();
                isModeForTab = false;
                handleUpdateUrl(vm, scope, compile);
                
            };
           
            vm.setDefaultTemplate = function () {

                if (vm.classSetDefault == "") {
                    vm.classSetDefault = "green";
                    scope.$emit('setDefaultTemplateFromTopHeaderE', true);
                    toastr.success("Template set as default");
                }
                else {
                    vm.classSetDefault = "";
                    scope.$emit('setDefaultTemplateFromTopHeaderE', false);
                    toastr.success("Template not set as default");
                }
            };
            vm.setPublicTemplate = function () {

                if (vm.classSetPublic == "") {
                    vm.classSetPublic = "green";
                    scope.$emit('setPublicTemplateFromTopHeaderE', true);
                }
                else {
                    vm.classSetPublic = "";
                    scope.$emit('setPublicTemplateFromTopHeaderE', false);
                }
            };
           

            $('[data-toggle="tooltip"]').tooltip();
            vm.printDashboardAndReport = function () {
                //$('body').addClass('when-print');
                //window.print();
                //$('body').removeClass('when-print');
                $('body').addClass('when-print');
                if ($(window).width() === 1536) {
                    $(".printSetting").css({ "zoom": "91%" });
                }
                setTimeout(function () { }, 1000)
                window.print();
                $('body').removeClass('when-print');
            };

            vm.LoadAllDataTemplate = function () {
                var rt = $("#ReportTemplateId option:selected").val();
                if (rt != 0) {
                    $.ajax({
                        url: "/Market/GetDataTemplates?id=" + rt,
                        type: "GET",
                        success: function (res) {
                            //
                            var DataTemplates = res;
                            vm.tableNViews = res;
                            if (vm.tableNViews.length > 0) {
                                vm.remoteDataSourceinfo['remoteDatabaseWhereClause'] = handleMakeWhereClauseForTablesNViews(vm);
                                handleGetRemoteDatabase(vm, scope, compile)
                                console.log(vm.remoteDataSourceinfo);
                                console.log('DataTemplates');
                                console.log(DataTemplates);
                            }
                        },
                        error: function (e) {
                            console.log("Error:" + e);
                        },
                        complete: function (msg) {

                        }
                    });
                }
            };
           // scope.$apply();
        };
        //#region lock grid stack
        var handleLockGridStack = function () {
            var lockGridStack = $('.grid-stack').data('gridstack');
            if (lockGridStack) {
                lockGridStack.enableMove(false, true);
                lockGridStack.enableResize(false, true);
            }

            if ($('.tab-content .tab-pane> .grid-stack').length > 0) {
                $('.tab-content .tab-pane> .grid-stack').each(function (idx, item) {

                    var lockGridStackChild = $(this).data('gridstack');
                    if (lockGridStackChild) {
                        lockGridStackChild.enableMove(false, true);
                        lockGridStackChild.enableResize(false, true);
                    }
                });
            }
        };
        //#endregion

        //#region unlock grid stack
        var handleUnlockGridStack = function () {
            var lockGridStack = $('.grid-stack').data('gridstack');
            if (lockGridStack) {
                lockGridStack.enableMove(true, true);
                lockGridStack.enableResize(true, true);
            }
            if ($('.tab-content .tab-pane> .grid-stack').length > 0) {
                $('.tab-content .tab-pane> .grid-stack').each(function (idx, item) {


                    var lockGridStackChild = $(this).data('gridstack');
                    if (lockGridStackChild) {
                        lockGridStackChild.enableMove(true, true);
                        lockGridStackChild.enableResize(true, true);
                    }
                });
            }

        };
        //#endregion
        return {
            initialization: function (vm, scope, compile, $uibModal) {
                hanldeInitialization(vm, scope, compile, $uibModal);

            }
        };
    }();
    return TopSectionDashboardAndReportServiceHandler;
});