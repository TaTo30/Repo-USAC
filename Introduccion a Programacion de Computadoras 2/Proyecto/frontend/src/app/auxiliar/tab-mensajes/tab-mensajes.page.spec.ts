import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabMensajesPage } from './tab-mensajes.page';

describe('TabMensajesPage', () => {
  let component: TabMensajesPage;
  let fixture: ComponentFixture<TabMensajesPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabMensajesPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabMensajesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
