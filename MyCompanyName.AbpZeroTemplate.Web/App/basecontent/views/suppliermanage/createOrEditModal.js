(function () {
    appModule.controller('basecontent.views.suppliermanage.createOrEditModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'userId', 'abp.services.app.supplier',
    function ($scope, $uibModalInstance, userService, userId, supplierService) {
        var vm = this;

        vm.saving = false;

        vm.isTwoFactorEnabled = abp.setting.getBoolean("Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled");
        vm.isLockoutEnabled = abp.setting.getBoolean("Abp.Zero.UserManagement.UserLockOut.IsEnabled");
        vm.supplier = null;
        vm.types = [{ id: 1, name: '一般' }, { id: 2, name: '特殊' }, { id: 3, name: 'VIP' }];

        vm.newsave = function () {
            vm.saving = true;
            supplierService.createOrUpdateSupplier(vm.supplier).then(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                $uibModalInstance.close();
            }).finally(function () {
                vm.saving = false;
            });
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss();
        };



        function newinit() {
            supplierService.getSupplierForEdit({
                id: userId
            }).then(function (result) {
                vm.supplier = result.data;
            });
        }
        newinit();

    }
    ]);
})();