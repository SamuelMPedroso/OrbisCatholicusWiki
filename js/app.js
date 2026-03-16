// ============================================================
// OrbisCatholicusWiki - JavaScript Principal
// ============================================================

document.addEventListener('DOMContentLoaded', function () {

    // === Sidebar Toggle (Mobile) ===
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');

    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', function () {
            sidebar.classList.toggle('open');
            if (sidebarOverlay) {
                sidebarOverlay.classList.toggle('open');
            }
        });
    }

    if (sidebarOverlay) {
        sidebarOverlay.addEventListener('click', function () {
            sidebar.classList.remove('open');
            sidebarOverlay.classList.remove('open');
        });
    }

    // === Sidebar Section Collapse ===
    document.querySelectorAll('.sidebar-section h3').forEach(function (header) {
        header.addEventListener('click', function () {
            this.parentElement.classList.toggle('collapsed');
        });
    });

    // === Scroll to Top Button ===
    const scrollTopBtn = document.getElementById('scrollTop');

    if (scrollTopBtn) {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 300) {
                scrollTopBtn.classList.add('visible');
            } else {
                scrollTopBtn.classList.remove('visible');
            }
        });

        scrollTopBtn.addEventListener('click', function () {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    // === Smooth Scroll for Anchor Links ===
    document.querySelectorAll('a[href^="#"]').forEach(function (anchor) {
        anchor.addEventListener('click', function (e) {
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;

            const target = document.querySelector(targetId);
            if (target) {
                e.preventDefault();
                const headerOffset = 70;
                const elementPosition = target.getBoundingClientRect().top;
                const offsetPosition = elementPosition + window.scrollY - headerOffset;

                window.scrollTo({
                    top: offsetPosition,
                    behavior: 'smooth'
                });

                // Close mobile sidebar if open
                if (sidebar && sidebar.classList.contains('open')) {
                    sidebar.classList.remove('open');
                    if (sidebarOverlay) {
                        sidebarOverlay.classList.remove('open');
                    }
                }
            }
        });
    });

    // === Search Input Focus Enhancement ===
    const searchInput = document.querySelector('.header-search input[type="search"]');
    if (searchInput) {
        searchInput.addEventListener('focus', function () {
            this.parentElement.style.flex = '0 1 500px';
        });
        searchInput.addEventListener('blur', function () {
            this.parentElement.style.flex = '0 1 400px';
        });
    }

    // === Category Cards Hover Effect ===
    document.querySelectorAll('.category-card').forEach(function (card) {
        card.addEventListener('mouseenter', function () {
            this.style.borderColor = 'var(--primary)';
        });
        card.addEventListener('mouseleave', function () {
            this.style.borderColor = '';
        });
    });

    // === Table of Contents Active Highlight ===
    const tocLinks = document.querySelectorAll('.toc a');
    const articleSections = document.querySelectorAll('.article-body h2[id], .article-body h3[id]');

    if (tocLinks.length > 0 && articleSections.length > 0) {
        window.addEventListener('scroll', function () {
            let current = '';

            articleSections.forEach(function (section) {
                const sectionTop = section.offsetTop - 100;
                if (window.scrollY >= sectionTop) {
                    current = section.getAttribute('id');
                }
            });

            tocLinks.forEach(function (link) {
                link.style.fontWeight = '';
                link.style.color = '';
                if (link.getAttribute('href') === '#' + current) {
                    link.style.fontWeight = '600';
                    link.style.color = 'var(--primary)';
                }
            });
        });
    }

    // === Reading Progress Bar (Article Pages) ===
    const articleBody = document.querySelector('.article-body');
    if (articleBody) {
        const progressBar = document.createElement('div');
        progressBar.style.cssText = 'position:fixed;top:0;left:0;height:3px;background:var(--gold);z-index:1001;transition:width 0.1s;width:0';
        document.body.appendChild(progressBar);

        window.addEventListener('scroll', function () {
            const articleRect = articleBody.getBoundingClientRect();
            const articleHeight = articleBody.offsetHeight;
            const scrolled = -articleRect.top;
            const progress = Math.min(Math.max(scrolled / (articleHeight - window.innerHeight), 0), 1);
            progressBar.style.width = (progress * 100) + '%';
        });
    }

});
