import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabForoPage } from './tab-foro.page';

describe('TabForoPage', () => {
  let component: TabForoPage;
  let fixture: ComponentFixture<TabForoPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabForoPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabForoPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
