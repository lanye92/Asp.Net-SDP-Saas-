/// <reference path="createOrEditModal.js" />
(function () {

    appModule.controller('basecontent.views.suppliermanage.index', [
        '$scope', '$uibModal', '$stateParams', 'uiGridConstants', 'abp.services.app.user', 'abp.services.app.supplier',
        function ($scope, $uibModal, $stateParams, uiGridConstants, userService, supplierService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.advancedFiltersAreShown = false;
            vm.filterText = $stateParams.filterText || '';
            vm.currentUserId = abp.session.userId;

            vm.permissions = {
                create: abp.auth.hasPermission('Pages.Tenant.Supplier.Create'),
                edit: abp.auth.hasPermission('Pages.Tenant.Supplier.Edit'),
                changePermissions: abp.auth.hasPermission('Pages.Administration.Users.ChangePermissions'),
                impersonation: abp.auth.hasPermission('Pages.Administration.Users.Impersonation'),
                'delete': abp.auth.hasPermission('Pages.Tenant.Supplier.Delete'),
                roles: abp.auth.hasPermission('Pages.Administration.Roles')
            };



            vm.requestParams = {
                permission: '',
                role: '',
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };



            vm.supplierGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: false,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: app.localize('Actions'),
                        enableSorting: false,
                        width: 120,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-if="grid.appScope.permissions.edit" ng-click="grid.appScope.editSupplier(row.entity)">' + app.localize('Edit') + '</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.delete" ng-click="grid.appScope.deleteSupplier(row.entity)">' + app.localize('Delete') + '</a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                     {
                         name: app.localize('UserName'),
                         field: 'userName',
                         cellTemplate:
                             '<div class=\"ui-grid-cell-contents\">' +
                             '  <img ng-if="row.entity.profilePictureId" ng-src="' + abp.appPath + 'Profile/GetProfilePictureById?id={{row.entity.profilePictureId}}" width="22" height="22" class="img-rounded img-profile-picture-in-grid" />' +
                             '  <img ng-if="!row.entity.profilePictureId" src="' + abp.appPath + 'Common/Images/default-profile-picture.png" width="22" height="22" class="img-rounded" />' +
                             '  {{COL_FIELD CUSTOM_FILTERS}} &nbsp;' +
                             '</div>',
                         minWidth: 140
                     },
                    {
                        name: app.localize('Address'),
                        field: 'address',
                        minWidth: 140
                    },
                    {
                        name: app.localize('Phone'),
                        field: 'phone',
                        minWidth: 120
                    },
                    {
                        name: app.localize('Type'),
                        field: 'type',
                        minWidth: 120
                    },
                    {
                        name: app.localize('CreationTime'),
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'L\'',
                        minWidth: 200
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = null;
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getSupplier();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getSupplier();
                    });
                },
                data: []
            };


            vm.getSupplier = function () {
                vm.loading - true;
                supplierService.getList($.extend({ filter: vm.filterText }, vm.requestParams)).then(function (result) {
                    vm.supplierGridOptions.totalItems = result.data.totalCount;
                    vm.supplierGridOptions.data = result.data.items;
                    console.log(result);
                }).finally(function () {
                    vm.loading = false;
                });
            }


            vm.editSupplier = function (supplier) {
                openCreateOrEditSupplierModal(supplier.id);
            }

            vm.createSupplier = function () {
                openCreateOrEditSupplierModal(null);
            };

            vm.deleteSupplier = function (supplier) {

                abp.message.confirm(
                    app.localize('UserDeleteWarningMessage', supplier.userName),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            supplierService.deleteSupplier({
                                id: supplier.id
                            }).then(function () {
                                vm.getSupplier();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            function openCreateOrEditSupplierModal(supplierid) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/basecontent/views/suppliermanage/createOrEditModal.cshtml',
                    controller: 'basecontent.views.suppliermanage.createOrEditModal as vm',
                    backdrop: 'static',
                    resolve: {
                        userId: function () {
                            return supplierid;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    vm.getSupplier();
                });
            }

            vm.getSupplier();
        }]);
})();