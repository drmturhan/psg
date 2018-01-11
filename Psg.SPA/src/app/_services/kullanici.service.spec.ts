/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { KullaniciService } from './kullanici.service';

describe('Service: Kullanici', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [KullaniciService]
    });
  });

  it('should ...', inject([KullaniciService], (service: KullaniciService) => {
    expect(service).toBeTruthy();
  }));
});