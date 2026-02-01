document.addEventListener('DOMContentLoaded', function() {
    // Auto dismiss alerts after 5 seconds
    setTimeout(function() {
        document.querySelectorAll('.alert-dismissible').forEach(function(alert) {
            try {
                var bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            } catch(e) {}
        });
    }, 5000);

    // Update cart count
    function updateCartCount() {
        fetch('/Koszyk/GetCount')
            .then(function(response) { return response.json(); })
            .then(function(data) {
                document.querySelectorAll('#koszykBadge, #cartBadge').forEach(function(el) {
                    el.textContent = data.count;
                    if (data.count > 0) {
                        el.style.display = 'inline';
                    } else {
                        el.style.display = 'none';
                    }
                });
            })
            .catch(function() {});
    }

    // Only update cart if user is logged in (cart endpoint exists)
    if (document.getElementById('koszykBadge') || document.getElementById('cartBadge')) {
        updateCartCount();
    }

    // Mobile sidebar toggle
    var sidebar = document.querySelector('.sidebar');
    if (sidebar) {
        document.addEventListener('click', function(e) {
            if (sidebar.classList.contains('show') && !sidebar.contains(e.target) && !e.target.closest('.mobile-menu-btn')) {
                sidebar.classList.remove('show');
            }
        });
    }

    // Add fade-in animation to content
    var contentArea = document.querySelector('.content-area');
    if (contentArea) {
        contentArea.classList.add('fade-in');
    }
});