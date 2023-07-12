import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabActividadesPage } from './tab-actividades.page';

describe('TabActividadesPage', () => {
  let component: TabActividadesPage;
  let fixture: ComponentFixture<TabActividadesPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabActividadesPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabActividadesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
