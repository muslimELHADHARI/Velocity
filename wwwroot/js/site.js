// ============================================
// VELOCITY - Premium JavaScript
// Smooth Animations & Interactions
// ============================================

const themeKey = "velocity-theme";

// Theme Management
const applyTheme = (theme) => {
    document.documentElement.setAttribute("data-bs-theme", theme);
    const icon = document.querySelector("#themeToggle i");
    if (icon) {
        icon.className = theme === "dark" ? "bi bi-sun-fill" : "bi bi-moon-fill";
    }
    localStorage.setItem(themeKey, theme);
    
    // Add transition class for smooth theme change
    document.body.classList.add("theme-transitioning");
    setTimeout(() => {
        document.body.classList.remove("theme-transitioning");
    }, 300);
};

// Initialize Theme
document.addEventListener("DOMContentLoaded", () => {
    const saved = localStorage.getItem(themeKey) || "light";
    applyTheme(saved);

    // Theme Toggle
    const toggle = document.getElementById("themeToggle");
    if (toggle) {
        toggle.addEventListener("click", () => {
            const current = document.documentElement.getAttribute("data-bs-theme") === "dark" ? "dark" : "light";
            applyTheme(current === "dark" ? "light" : "dark");
            
            // Add ripple effect
            const ripple = document.createElement("span");
            ripple.style.cssText = `
                position: absolute;
                border-radius: 50%;
                background: rgba(255, 107, 53, 0.3);
                width: 100px;
                height: 100px;
                margin-top: -50px;
                margin-left: -50px;
                animation: ripple 0.6s ease-out;
                pointer-events: none;
            `;
            toggle.appendChild(ripple);
            setTimeout(() => ripple.remove(), 600);
        });
    }

    // Page Load Animation
    document.body.classList.add("page-loaded");
    
    // Intersection Observer for Scroll Animations
    initScrollAnimations();
    
    // Smooth Scroll for Anchor Links
    initSmoothScroll();
    
    // Form Enhancements
    initFormEnhancements();
    
    // Card Hover Effects
    initCardEffects();
});

// Scroll Animations
function initScrollAnimations() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: "0px 0px -50px 0px"
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = "1";
                entry.target.style.transform = "translateY(0)";
            }
        });
    }, observerOptions);

    // Animate cards on scroll
    document.querySelectorAll(".vehicle-card, .card-modern").forEach(card => {
        card.style.opacity = "0";
        card.style.transform = "translateY(30px)";
        card.style.transition = "opacity 0.6s ease-out, transform 0.6s ease-out";
        observer.observe(card);
    });
}

// Smooth Scroll
function initSmoothScroll() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener("click", function (e) {
            const href = this.getAttribute("href");
            if (href !== "#" && href.startsWith("#")) {
                e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({
                        behavior: "smooth",
                        block: "start"
                    });
                }
            }
        });
    });
}

// Form Enhancements
function initFormEnhancements() {
    // Floating Labels Animation
    const inputs = document.querySelectorAll(".form-group-floating input");
    inputs.forEach(input => {
        input.addEventListener("focus", function() {
            this.parentElement.classList.add("focused");
        });
        
        input.addEventListener("blur", function() {
            if (!this.value) {
                this.parentElement.classList.remove("focused");
            }
        });
        
        // Check if input has value on load
        if (input.value) {
            input.parentElement.classList.add("focused");
        }
    });
    
    // Form Submit Animation
    const forms = document.querySelectorAll("form");
    forms.forEach(form => {
        form.addEventListener("submit", function(e) {
            const submitBtn = this.querySelector('button[type="submit"]');
            if (submitBtn && !submitBtn.disabled) {
                submitBtn.style.transform = "scale(0.95)";
                setTimeout(() => {
                    submitBtn.style.transform = "scale(1)";
                }, 150);
            }
        });
    });
}

// Card Hover Effects - Reduced for better UX
function initCardEffects() {
    // Only apply subtle effects to vehicle cards, not reservation cards or tables
    const cards = document.querySelectorAll(".vehicle-card");
    cards.forEach(card => {
        // Remove the 3D tilt effect - just use CSS hover
        card.addEventListener("mouseenter", function() {
            this.style.transition = "all 0.2s ease";
        });
    });
}

// Navbar Scroll Effect
let lastScroll = 0;
const navbar = document.querySelector(".navbar-wrapper");

window.addEventListener("scroll", () => {
    const currentScroll = window.pageYOffset;
    
    if (currentScroll > 100) {
        navbar.style.boxShadow = "0 4px 16px rgba(255, 107, 53, 0.2)";
    } else {
        navbar.style.boxShadow = "0 2px 8px rgba(255, 107, 53, 0.08)";
    }
    
    lastScroll = currentScroll;
});

// Add Ripple Effect CSS
const style = document.createElement("style");
style.textContent = `
    @keyframes ripple {
        to {
            transform: scale(4);
            opacity: 0;
        }
    }
    
    .theme-transitioning {
        transition: background-color 0.3s ease, color 0.3s ease !important;
    }
`;
document.head.appendChild(style);

// Button Click Effects
document.querySelectorAll(".btn-primary-gradient").forEach(btn => {
    btn.addEventListener("click", function(e) {
        const ripple = document.createElement("span");
        const rect = this.getBoundingClientRect();
        const size = Math.max(rect.width, rect.height);
        const x = e.clientX - rect.left - size / 2;
        const y = e.clientY - rect.top - size / 2;
        
        ripple.style.cssText = `
            position: absolute;
            width: ${size}px;
            height: ${size}px;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.5);
            left: ${x}px;
            top: ${y}px;
            transform: scale(0);
            animation: ripple 0.6s ease-out;
            pointer-events: none;
        `;
        
        this.appendChild(ripple);
        setTimeout(() => ripple.remove(), 600);
    });
});

// Loading Animation
window.addEventListener("load", () => {
    document.body.classList.add("loaded");
    
    // Animate elements on page load
    setTimeout(() => {
        document.querySelectorAll(".fade-in-up").forEach((el, index) => {
            setTimeout(() => {
                el.style.opacity = "1";
                el.style.transform = "translateY(0)";
            }, index * 100);
        });
    }, 100);
});

// Console Welcome Message
console.log("%c🚀 Velocity - Premium Bike & Motor Rental Platform", "font-size: 20px; font-weight: bold; color: #FF6B35;");
console.log("%cBuilt with passion and attention to detail", "font-size: 12px; color: #FFB627;");
