import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

const initTooltips = (): void => {
  const bootstrapGlobal = (window as typeof window & { bootstrap?: any }).bootstrap;
  const TooltipCtor = bootstrapGlobal?.Tooltip;
  if (!TooltipCtor) {
    return;
  }

  const tooltipTargets = Array.from(
    document.querySelectorAll('[data-bs-toggle="tooltip"]')
  ) as HTMLElement[];

  tooltipTargets.forEach((element) => {
    if (element.getAttribute('data-bs-tooltip-initialized') === 'true') {
      return;
    }
    new TooltipCtor(element);
    element.setAttribute('data-bs-tooltip-initialized', 'true');
  });
};

document.addEventListener('DOMContentLoaded', initTooltips);
window.addEventListener('bootstrap:refresh-tooltips', initTooltips);

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
