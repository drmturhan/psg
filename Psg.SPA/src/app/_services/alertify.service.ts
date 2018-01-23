import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable()
export class AlertifyService {

  constructor() { }
  confirm(message: string, okCallback: () => any, title?: string, evet?: string, iptal?: string) {
    let evetStr: string = 'Evet';
    let iptalStr: string = 'iptal';
    if (evet)
      evetStr = evet;
    if (iptal)
      iptalStr = iptal;

    if (!title)
      title = 'Onay';

    alertify.confirm()
      .setting({
        'labels': { ok: evetStr, cancel: iptalStr },
        'message': message,
        'title': title,
        'onok': okCallback,
        'transition': 'zoom'
      }).show();
  }

  success(message: string) {
    alertify.success(message);
  }
  error(message: string) {
      alertify.set('notifier','position', 'bottom-center');
    alertify.error(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }
  message(message: string) {
    alertify.message(message);
  }
}
