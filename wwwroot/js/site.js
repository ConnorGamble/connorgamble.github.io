(() => {
    // Mobile menu
    const header = document.querySelector('[data-menu]');
    const toggle = document.querySelector('[data-menu-toggle]');

    const setMenu = open => {
        header.classList.toggle('is-open', open);
        document.body.classList.toggle('menu-open', open);
        toggle.setAttribute('aria-expanded', String(open));
        toggle.setAttribute('aria-label', open ? 'Close menu' : 'Open menu');
    };

    if (header && toggle) {
        toggle.addEventListener('click', () => setMenu(!header.classList.contains('is-open')));
        header.querySelectorAll('.mobile-menu a').forEach(a => a.addEventListener('click', () => setMenu(false)));
        document.addEventListener('keydown', e => e.key === 'Escape' && setMenu(false));
        matchMedia('(min-width: 721px)').addEventListener('change', e => e.matches && setMenu(false));
    }

    // Reveal elements as they scroll into view
    const revealables = document.querySelectorAll('.reveal');
    if ('IntersectionObserver' in window) {
        const observer = new IntersectionObserver(entries => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('is-visible');
                    observer.unobserve(entry.target);
                }
            });
        }, { rootMargin: '0px 0px -40px 0px' });
        revealables.forEach(el => observer.observe(el));
    } else {
        revealables.forEach(el => el.classList.add('is-visible'));
    }

    // Card glow follows the pointer
    document.querySelectorAll('.card').forEach(card => {
        card.addEventListener('pointermove', e => {
            const rect = card.getBoundingClientRect();
            card.style.setProperty('--mx', `${e.clientX - rect.left}px`);
            card.style.setProperty('--my', `${e.clientY - rect.top}px`);
        });
    });

    // Code highlighting
    if (window.hljs) {
        hljs.highlightAll();
    }
})();
