
$(document).ready(function () {
    $('#Poster').on('change', function () {
        var selectedFile = $(this).val().split('\\').pop();
        $(this).siblings('label').text(selectedFile);

        var posterContainer = $('#poster-container');
        var image = window.URL.createObjectURL(this.files[0]);

        posterContainer.removeClass('d-none');
        posterContainer.find('img').attr('src', image);
    });

    $('#Year').datepicker({
        format: 'yyyy',
        viewMode: 'years',
        minViewMode: 'years',
        autoclose: true,
        startDate: new Date('1950-01-01'),
        endDate: new Date()
    });
});

$('.js-delete').on('click', function () {
        var btn = $(this);

        bootbox.confirm({
            message: "Are you sure that you need to delete this movie?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-outline-secondary'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        url: '/movies/delete/' + btn.data('id'),
                        success: function () {
                            var movieContainer = btn.parents('.col-12');
                            movieContainer.addClass('animate__animated animate__zoomOut');

                            setTimeout(function () {
                                movieContainer.remove();
                            }, 1000);

                            toastr.success('Movies deleted');
                        },
                        error: function () {
                            toastr.error('Something went wrong!');
                        }
                    });
                }
            }
        });
    });
