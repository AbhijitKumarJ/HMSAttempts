# HMS Core Design System - Developer Implementation Guide

## Overview
This guide provides developers with exact class specifications and design token references for maintaining and extending the modernized HMS Core UI design.

---

## Color System

### Primary Healthcare Palette
```css
/* Cyan - Primary brand color for Receptionist/Doctor roles */
--cyan-50:    #F0F9FC;
--cyan-100:   #E0F2FE;
--cyan-200:   #BAE6FD;
--cyan-300:   #7DD3FC;
--cyan-400:   #22D3EE;
--cyan-500:   #06B6D4;  /* Primary */
--cyan-600:   #0891B2;  /* Dark variant */
--cyan-700:   #0E7490;

/* Emerald - Secondary for Nurse role & success states */
--emerald-50:   #F0FDF4;
--emerald-100:  #DCFCE7;
--emerald-200:  #BBF7D0;
--emerald-300:  #86EFAC;
--emerald-400:  #4ADE80;
--emerald-500:  #22C55E;
--emerald-600:  #16A34A;
--emerald-700:  #15803D;
--emerald-800:  #166534;  /* Dark variant */
--emerald-900:  #14532D;

/* Slate - Neutral & text colors */
--slate-50:     #F8FAFC;
--slate-100:    #F1F5F9;
--slate-200:    #E2E8F0;
--slate-300:    #CBD5E1;
--slate-400:    #94A3B8;
--slate-500:    #64748B;
--slate-600:    #475569;
--slate-700:    #334155;
--slate-800:    #1E293B;  /* Sidebar base */
--slate-900:    #0F172A;  /* Dark background */
--slate-950:    #020617;  /* Darkest */

/* Indigo - Admin role */
--indigo-50:    #EEF2FF;
--indigo-100:   #E0E7FF;
--indigo-400:   #818CF8;
--indigo-500:   #6366F1;
--indigo-600:   #4F46E5;
--indigo-700:   #4338CA;
```

### Role-Based Color Mapping
```jsx
// In component themeConfig computed:

Receptionist: {
  sidebar: 'bg-gradient-to-b from-slate-800 to-slate-900',
  sidebarHeader: 'bg-slate-950',
  logo: 'bg-gradient-to-br from-cyan-500 to-teal-600',
  roleBadge: 'bg-cyan-100 text-cyan-800',
  accent: 'cyan'
}

Nurse: {
  sidebar: 'bg-gradient-to-b from-emerald-800 to-emerald-900',
  sidebarHeader: 'bg-emerald-950',
  logo: 'bg-gradient-to-br from-emerald-500 to-green-600',
  roleBadge: 'bg-emerald-100 text-emerald-800',
  accent: 'emerald'
}

Doctor: {
  sidebar: 'bg-gradient-to-b from-slate-800 to-slate-900',
  sidebarHeader: 'bg-slate-950',
  logo: 'bg-gradient-to-br from-cyan-500 to-blue-600',
  roleBadge: 'bg-cyan-100 text-cyan-800',
  accent: 'cyan'
}

Admin: {
  sidebar: 'bg-gradient-to-b from-slate-800 to-slate-900',
  sidebarHeader: 'bg-slate-950',
  logo: 'bg-gradient-to-br from-indigo-500 to-purple-600',
  roleBadge: 'bg-indigo-100 text-indigo-800',
  accent: 'indigo'
}
```

---

## Typography System

### Font Stack
```css
font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 
             'Oxygen', 'Ubuntu', 'Cantarell', 'Fira Sans', 'Droid Sans', 
             'Helvetica Neue', sans-serif;
```

### Type Scale
```css
/* Headings */
h1: font-size: 2.5rem; font-weight: 700; line-height: 1.2;   /* 40px */
h2: font-size: 2rem;   font-weight: 700; line-height: 1.3;   /* 32px */
h3: font-size: 1.5rem; font-weight: 600; line-height: 1.4;   /* 24px */
h4: font-size: 1.25rem; font-weight: 600; line-height: 1.4;  /* 20px */

/* Body */
body: font-size: 0.875rem; font-weight: 400; line-height: 1.5; /* 14px */
p:   font-size: 0.875rem; font-weight: 400; line-height: 1.6; /* 14px */

/* Small text */
small: font-size: 0.75rem; font-weight: 400; line-height: 1.5; /* 12px */
label: font-size: 0.875rem; font-weight: 600; line-height: 1.4; /* 14px */
```

### Font Weights
```css
--font-light:    300;
--font-normal:   400;
--font-medium:   500;
--font-semibold: 600;
--font-bold:     700;
--font-extrabold: 800;
```

### Usage Examples
```html
<!-- Logo -->
<span class="text-xl font-bold tracking-tight">HMS Core</span>
<!-- Navigation items -->
<a class="text-sm font-medium">Dashboard</a>
<!-- Form labels -->
<label class="block text-sm font-semibold text-slate-900">Email</label>
<!-- Card titles -->
<h6 class="text-lg font-bold text-slate-900">Patient Information</h6>
<!-- Body text -->
<p class="text-sm text-slate-600">Description text here</p>
```

---

## Spacing System

### Space Scale
```css
--space-0:    0px;
--space-1:    0.25rem;  /* 4px */
--space-2:    0.5rem;   /* 8px */
--space-3:    0.75rem;  /* 12px */
--space-4:    1rem;     /* 16px */
--space-5:    1.25rem;  /* 20px */
--space-6:    1.5rem;   /* 24px */
--space-8:    2rem;     /* 32px */
--space-10:   2.5rem;   /* 40px */
--space-12:   3rem;     /* 48px */
--space-16:   4rem;     /* 64px */
```

### Component Spacing
```
Sidebar:
  px: 3 (horizontal), 4 (nav items)
  py: 6 (container), 3 (items)
  space-y: 1 (gap between items)

Header:
  px: 8
  h: 16 (height)
  space-x: 4, 6 (gaps)

Main Content:
  p: 6, 8 (padding)
  gap: 6, 8 (between sections)

Cards:
  p: 8 (internal padding)
  gap: 6 (between cards)
  border: 1px

Forms:
  space-y: 6 (between form sections)
  py: 2.5 (input vertical)
  px: 4 (input horizontal)
```

### Touch Target Sizing
```css
Minimum button size: 44×44px
  = py-3 px-4 (for square buttons)
  = py-2.5 px-5 (for wider buttons)
  
Navigation items: 48×44px
  = py-3 px-4
  
Input fields: 40×44px minimum
  = py-2.5 px-4
```

---

## Shadow System

### Shadow Scale
```css
--shadow-sm:  0 1px 2px 0 rgba(0, 0, 0, 0.05);
--shadow-md:  0 4px 6px -1px rgba(0, 0, 0, 0.1);
--shadow-lg:  0 10px 15px -3px rgba(0, 0, 0, 0.1);
--shadow-xl:  0 20px 25px -5px rgba(0, 0, 0, 0.1);

Applied to:
shadow-sm:  Cards (default), inputs (default)
shadow-md:  Header, elevated cards, hero section
shadow-lg:  Modals, popovers, floating elements
```

### Healthcare-Specific Shadows
```css
/* Subtle depth for cards */
box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);

/* Logo enhancement */
box-shadow: 0 4px 12px rgba(8, 145, 178, 0.3);

/* Button elevation on hover */
box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
```

---

## Border Radius System

### Radius Scale
```css
--radius-none:    0px;
--radius-sm:      0.125rem;  /* 2px */
--radius-base:    0.25rem;   /* 4px */
--radius-md:      0.375rem;  /* 6px */
--radius-lg:      0.5rem;    /* 8px */
--radius-xl:      0.75rem;   /* 12px */
--radius-2xl:     1rem;      /* 16px */
--radius-3xl:     1.5rem;    /* 24px */
--radius-full:    9999px;    /* Pill shape */

Component Usage:
Cards:      rounded-2xl
Buttons:    rounded-lg
Inputs:     rounded-lg
Logo:       rounded-xl or rounded-2xl
Badges:     rounded-full
```

### Application
```html
<!-- Card with rounded-2xl -->
<div class="rounded-2xl shadow-md border border-cyan-100 p-8"></div>

<!-- Button with rounded-lg -->
<button class="rounded-lg px-5 py-2.5"></button>

<!-- Logo with rounded-xl -->
<div class="rounded-xl w-10 h-10 bg-gradient-to-br from-cyan-500 to-teal-600"></div>

<!-- Badge with rounded-full -->
<span class="rounded-full px-3 py-1 text-xs font-bold bg-cyan-100 text-cyan-800"></span>
```

---

## Component Specifications

### Navigation Link (Active)
```html
<!-- OLD (Pre-modernization) -->
<a routerLinkActive="bg-blue-600 text-white shadow-lg" 
   class="px-3 py-2.5 text-sm font-medium rounded-lg">

<!-- NEW (Modernized) -->
<a routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400"
   class="group flex items-center px-4 py-3 text-sm font-medium rounded-lg 
          text-slate-200 hover:bg-white/10 hover:text-white 
          transition-all duration-200 
          focus:outline-none focus:ring-2 focus:ring-cyan-400">
```

**Key Changes:**
- ✅ Increased padding: `py-2.5` → `py-3`, `px-3` → `px-4`
- ✅ Active indicator: Left border `border-l-4 border-{color}-400`
- ✅ Subtle background: `bg-{color}-500/20` (20% opacity)
- ✅ Focus ring: `focus:ring-2 focus:ring-{color}-400`
- ✅ Transitions: `transition-all duration-200`

### Button (Primary)
```html
<button class="px-5 py-2.5 rounded-lg font-semibold text-white 
               bg-gradient-to-r from-cyan-500 to-teal-600 
               hover:from-cyan-600 hover:to-teal-700
               focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-cyan-400
               shadow-md hover:shadow-lg
               transition-all duration-200">
  Action
</button>
```

**Specifications:**
- Padding: `px-5 py-2.5` (44px minimum height)
- Border radius: `rounded-lg`
- Font: `font-semibold` text-sm
- Colors: Gradient primary to secondary
- Hover: Darker gradient + shadow increase
- Focus: `ring-2` with 2px offset
- Shadow: `shadow-md` base, `shadow-lg` on hover

### Form Input
```html
<input type="text"
       class="px-4 py-2.5 text-sm border border-slate-300 rounded-lg
              bg-slate-50 placeholder-slate-400
              focus:outline-none focus:ring-2 focus:ring-cyan-500 focus:border-transparent
              transition-all duration-200">
```

**Specifications:**
- Padding: `px-4 py-2.5` (40px+ height)
- Border: `border border-slate-300`
- Radius: `rounded-lg`
- Background: `bg-slate-50` (light neutral)
- Focus: `ring-2 ring-{role-color}-500`
- Transition: `transition-all duration-200`
- Border on focus: `border-transparent` (focus ring replaces it)

### Card Container
```html
<div class="rounded-2xl shadow-md border border-cyan-100 p-8
            bg-white hover:shadow-lg hover:border-cyan-200
            transition-all duration-300">
</div>
```

**Specifications:**
- Radius: `rounded-2xl`
- Border: `border border-{role-color}-100`
- Padding: `p-8`
- Background: `bg-white` or `bg-gradient-to-br from-slate-50 to-{color}-50`
- Shadow: `shadow-md` base, `shadow-lg` on hover
- Transition: `transition-all duration-300`

---

## Gradient Specifications

### Logo Gradients (by role)
```css
Receptionist: linear-gradient(135deg, #0891B2, #14B8A6)  /* Cyan → Teal */
Nurse:        linear-gradient(135deg, #059669, #10B981)  /* Emerald → Green */
Doctor:       linear-gradient(135deg, #0891B2, #2563EB)  /* Cyan → Blue */
Admin:        linear-gradient(135deg, #6366F1, #A855F7)  /* Indigo → Purple */
```

### Button Gradients
```css
Primary CTA:  linear-gradient(to right, #06B6D4, #14B8A6)    /* Cyan → Teal */
Secondary:    linear-gradient(to right, #059669, #10B981)    /* Emerald → Green */
Admin:        linear-gradient(to right, #6366F1, #A855F7)    /* Indigo → Purple */
```

### Background Gradients
```css
Landing page:   linear-gradient(to bottom right, 
                               from: #F8FAFC,    /* slate-50 */
                               via: #F0F9FC,     /* cyan-50 */
                               to: #F0FDF4)      /* emerald-50 */

Main app:       linear-gradient(to bottom right,
                               from: #F8FAFC,    /* slate-50 */
                               via: #FFFFFF,     /* white */
                               to: #F0F9FC)      /* cyan-50/40 */

Sidebar:        linear-gradient(to bottom,
                               from: #1E293B,    /* slate-800 */
                               to: #0F172A)      /* slate-900 */
```

---

## State Definitions

### Button States
```css
/* Default */
.btn { background: linear-gradient(to right, #06B6D4, #14B8A6); }

/* Hover */
.btn:hover { background: linear-gradient(to right, #0891B2, #0D9488); 
             box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); }

/* Focus */
.btn:focus { outline: none; 
             box-shadow: 0 0 0 4px rgba(6, 182, 212, 0.5); }

/* Active/Pressed */
.btn:active { transform: scale(0.95); }

/* Disabled */
.btn:disabled { opacity: 0.5; cursor: not-allowed; }
```

### Link States
```css
/* Default */
a { color: #0891B2; text-decoration: none; }

/* Hover */
a:hover { color: #06B6D4; transition: color 200ms ease-in-out; }

/* Focus */
a:focus { outline: 2px solid #0891B2; outline-offset: 2px; }

/* Visited */
a:visited { color: #0E7490; }
```

### Form Input States
```css
/* Default */
input { border: 1px solid #CBD5E1; background: #F1F5F9; }

/* Focus */
input:focus { border: 1px solid transparent; 
              ring: 2px #06B6D4; }

/* Error */
input.error { border: 1px solid #DC2626; 
              ring: 2px #FCA5A5; }

/* Disabled */
input:disabled { opacity: 0.5; cursor: not-allowed; }
```

---

## Animation & Transition Guidelines

### Duration Standards
```css
Fast micro-interactions:  150ms (focus rings, hovers)
Standard UI transitions:  200ms (default for most interactions)
Smooth page transitions:  300ms-400ms (longer, more noticeable)
Loading animations:       continuous (looping spinners)
```

### Easing Functions
```css
ease-in-out:  cubic-bezier(0.4, 0, 0.2, 1)  /* Natural, smooth */
ease-in:      cubic-bezier(0.4, 0, 1, 1)    /* Entrance */
ease-out:     cubic-bezier(0, 0, 0.2, 1)    /* Exit */
linear:       1, 0, 0, 1                     /* Avoid for UI */
```

### Recommended Transitions
```css
/* Hover effects */
transition: all 200ms ease-in-out;

/* Color changes */
transition: color 200ms ease-in-out, 
            background 200ms ease-in-out;

/* Visibility changes */
transition: opacity 200ms ease-in-out,
            transform 200ms ease-in-out;

/* Complex animations */
transition: all 300ms cubic-bezier(0.4, 0, 0.2, 1);
```

---

## Implementation Checklist

When building new components or modifying existing ones:

### Color & Styling
- [ ] Use role-specific colors from palette
- [ ] Minimum color contrast 4.5:1
- [ ] Apply appropriate shadow for depth
- [ ] Use correct border radius (lg/xl/2xl)
- [ ] Gradient logos match role colors

### Spacing
- [ ] Padding/margin follows space scale
- [ ] Touch targets minimum 44×44px
- [ ] Navigation items py-3 px-4
- [ ] Cards p-8, gap-6
- [ ] Form inputs py-2.5 px-4

### Typography
- [ ] Font size 14px minimum for body text
- [ ] Line height 1.5-1.75 for readability
- [ ] Proper font weights (400, 500, 600, 700)
- [ ] Consistent heading hierarchy
- [ ] Form labels font-semibold

### Accessibility
- [ ] Focus rings on all interactive elements
- [ ] Keyboard navigation supported
- [ ] Color not sole indicator
- [ ] Alt text for images
- [ ] Proper heading hierarchy

### Interactions
- [ ] Hover effects on interactive elements
- [ ] Active states for navigation
- [ ] Loading states with visual feedback
- [ ] Error states clearly marked
- [ ] Smooth transitions (200ms)

---

## Code Examples

### Creating a Modern Navigation Item
```html
<a routerLink="/app/patients" 
   routerLinkActive="bg-cyan-500/20 text-white border-l-4 border-cyan-400"
   class="group flex items-center px-4 py-3 text-sm font-medium 
          rounded-lg text-slate-200 
          hover:bg-white/10 hover:text-white 
          transition-all duration-200 
          focus:outline-none focus:ring-2 focus:ring-cyan-400">
  <svg class="mr-3 h-5 w-5 flex-shrink-0"><!-- icon --></svg>
  Patient Registry
</a>
```

### Creating a Modern Button
```html
<button class="px-5 py-2.5 rounded-lg font-semibold text-white 
               bg-gradient-to-r from-cyan-500 to-teal-600 
               hover:from-cyan-600 hover:to-teal-700
               focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-cyan-400
               shadow-md hover:shadow-lg
               transition-all duration-200
               disabled:opacity-50 disabled:cursor-not-allowed">
  Save Changes
</button>
```

### Creating a Modern Card
```html
<div class="rounded-2xl shadow-md border border-cyan-100 
            bg-white hover:shadow-lg hover:border-cyan-200
            transition-all duration-300 p-8">
  <h3 class="text-lg font-bold text-slate-900 mb-3">Card Title</h3>
  <p class="text-sm text-slate-600 leading-relaxed">
    Card content goes here
  </p>
</div>
```

---

## Maintenance & Updates

### When Updating Designs
1. Update this spec document first
2. Apply changes to components
3. Test accessibility (WCAG AA)
4. Verify across browsers
5. Update related documentation
6. Communicate changes to team

### Color Palette Updates
- Update color values in `styles.css` or design tokens
- Regenerate Tailwind CSS if using custom colors
- Update role-based theme mappings
- Test contrast ratios against all colors

### New Component Development
- Follow spacing scale (1, 2, 3, 4, 6, 8...)
- Use semantic HTML
- Include focus states
- Apply appropriate shadows
- Test on mobile devices
- Include accessibility features

---

**Document Version:** 1.0  
**Last Updated:** January 23, 2026  
**Status:** Active Implementation Guide
