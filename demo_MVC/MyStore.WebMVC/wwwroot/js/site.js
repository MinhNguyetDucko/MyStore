// MyStore JavaScript Functions

$(document).ready(function () {
    // Initialize page
    initializeMyStore();
});

function initializeMyStore() {
    // Add smooth scrolling for anchor links
    addSmoothScrolling();
    
    // Initialize search functionality
    initializeSearch();
    
    // Initialize navigation active states
    initializeNavigation();
    
    // Initialize cart functionality
    initializeCart();
    
    // Add fade-in animation to elements
    addFadeInAnimation();
}

// Smooth scrolling for anchor links
function addSmoothScrolling() {
    $('a[href^="#"]').on('click', function (e) {
        e.preventDefault();
        const target = $(this.getAttribute('href'));
        if (target.length) {
            $('html, body').animate({
                scrollTop: target.offset().top - 100
            }, 500);
        }
    });
}

// Search functionality
function initializeSearch() {
    const searchInput = $('#searchInput');
    
    searchInput.on('keypress', function (e) {
        if (e.which === 13) { // Enter key
            performSearch($(this).val());
        }
    });
    
    // Search on focus/blur effects
    searchInput.on('focus', function () {
        $(this).parent().addClass('search-active');
    });
    
    searchInput.on('blur', function () {
        $(this).parent().removeClass('search-active');
    });
}

function performSearch(query) {
    if (query.trim() !== '') {
        // Redirect to search results page
        window.location.href = `/Products?search=${encodeURIComponent(query)}`;
    }
}

// Navigation functionality
function initializeNavigation() {
    $('.nav-link').on('click', function () {
        // Remove active class from all links
        $('.nav-link').removeClass('active');
        // Add active class to clicked link
        $(this).addClass('active');
    });
}

// Cart functionality
function initializeCart() {
    // Update cart count
    updateCartCount();
    
    // Cart button click handler
    $('.action-btn[title="Giỏ hàng"]').on('click', function (e) {
        e.preventDefault();
        showCartSummary();
    });
}

function updateCartCount() {
    // This would typically get data from server
    // For now, we'll use a placeholder
    const cartCount = $('#cartCount').text() || '0';
    if (parseInt(cartCount) > 0) {
        $('#cartCount').show();
    } else {
        $('#cartCount').hide();
    }
}

function showCartSummary() {
    // This would show a cart summary modal or redirect to cart page
    const cartCount = $('#cartCount').text() || '0';
    if (parseInt(cartCount) > 0) {
        window.location.href = '/Cart';
    } else {
        showNotification('Giỏ hàng của bạn đang trống!', 'info');
    }
}

// Add to cart function (for product pages)
function addToCart(productId, quantity = 1) {
    $.ajax({
        url: '/Cart/Add',
        type: 'POST',
        data: {
            productId: productId,
            quantity: quantity
        },
        success: function (response) {
            if (response.success) {
                updateCartCount();
                showNotification('Đã thêm sản phẩm vào giỏ hàng!', 'success');
            } else {
                showNotification('Có lỗi xảy ra khi thêm sản phẩm!', 'error');
            }
        },
        error: function () {
            showNotification('Có lỗi xảy ra khi thêm sản phẩm!', 'error');
        }
    });
}

// Notification system
function showNotification(message, type = 'info') {
    const notification = $(`
        <div class="notification notification-${type}">
            <i class="fas fa-${getNotificationIcon(type)}"></i>
            <span>${message}</span>
            <button class="notification-close">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `);
    
    $('body').append(notification);
    
    // Animate in
    notification.addClass('show');
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        notification.removeClass('show');
        setTimeout(() => notification.remove(), 300);
    }, 5000);
    
    // Manual close
    notification.find('.notification-close').on('click', function () {
        notification.removeClass('show');
        setTimeout(() => notification.remove(), 300);
    });
}

function getNotificationIcon(type) {
    switch (type) {
        case 'success': return 'check-circle';
        case 'error': return 'exclamation-circle';
        case 'warning': return 'exclamation-triangle';
        default: return 'info-circle';
    }
}

// Fade-in animation for elements
function addFadeInAnimation() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver(function (entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('fade-in-up');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);
    
    // Observe elements that should animate
    document.querySelectorAll('.feature-card, .product-card, .content-section').forEach(el => {
        observer.observe(el);
    });
}

// Form validation helper
function validateForm(formId) {
    const form = $(`#${formId}`);
    let isValid = true;
    
    form.find('input[required], select[required], textarea[required]').each(function () {
        const field = $(this);
        const value = field.val().trim();
        
        if (value === '') {
            field.addClass('is-invalid');
            isValid = false;
        } else {
            field.removeClass('is-invalid');
        }
    });
    
    // Email validation
    form.find('input[type="email"]').each(function () {
        const field = $(this);
        const email = field.val().trim();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (email !== '' && !emailRegex.test(email)) {
            field.addClass('is-invalid');
            isValid = false;
        }
    });
    
    return isValid;
}

// Price formatting
function formatPrice(price) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(price);
}

// Loading spinner
function showLoading() {
    const loading = $(`
        <div class="loading-overlay">
            <div class="loading-spinner">
                <i class="fas fa-spinner fa-spin"></i>
                <p>Đang tải...</p>
            </div>
        </div>
    `);
    $('body').append(loading);
}

function hideLoading() {
    $('.loading-overlay').remove();
}

// Window scroll handler
$(window).on('scroll', function () {
    const scrollTop = $(this).scrollTop();
    
    // Add shadow to header when scrolling
    if (scrollTop > 50) {
        $('.header').addClass('scrolled');
    } else {
        $('.header').removeClass('scrolled');
    }
});