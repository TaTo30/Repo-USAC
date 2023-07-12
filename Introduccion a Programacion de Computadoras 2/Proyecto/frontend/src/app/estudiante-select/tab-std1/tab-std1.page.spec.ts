import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabStd1Page } from './tab-std1.page';

describe('TabStd1Page', () => {
  let component: TabStd1Page;
  let fixture: ComponentFixture<TabStd1Page>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabStd1Page ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabStd1Page);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
