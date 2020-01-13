// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
///  READY
$$ = jQuery;

$$.when($$.ready).then(function () {
    $$('.date').data('provide', 'datepicker').datepicker({
        format: 'dd/mm/yyyy',
        startDate: '-1d',
        language: 'es',
        todayHighlight: true,
        toggleActive: true
    });
    $$('.date').data('provide', 'datepicker').datepicker('update', new Date());
    $$('.date').data('provide', 'datepicker').datepicker()
        .on('changeDate', function (e) {
            $$('.date').data('provide', 'datepicker').datepicker('hide');
        });
});