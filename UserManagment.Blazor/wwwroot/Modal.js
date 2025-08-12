window.bootstrapInterop = {
    showModal: function (id) {
        var modalEl = document.querySelector(id);
        var modal = new bootstrap.Modal(modalEl);
        modal.show();
    },
    hideModal: function (id) {
        var modalEl = document.querySelector(id);
        var modal = bootstrap.Modal.getInstance(modalEl);
        if (modal) modal.hide();
    }
};
