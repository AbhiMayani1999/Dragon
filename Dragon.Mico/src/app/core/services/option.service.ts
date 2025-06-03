import { Injectable } from '@angular/core';
import { OptionTransfer } from '@core/models/dyna.model';
import { Modules } from '@urls';
import { Subject } from 'rxjs';
import { DataService } from './data.service';

@Injectable({ providedIn: 'root' })
export class OptionService {
  public optionTransferList: OptionTransfer[] = [];
  public optionSubject: Subject<boolean> = new Subject;
  constructor(public dataService: DataService) { }

  public pushTransfer(option: OptionTransfer) {
    const filteredOption = this.optionTransferList.find(o => (o.table === option.table && o.key === option.key && o.value === option.value) || (o.keystore === option.keystore) || o.keystore === option.keystore);
    if (!filteredOption) { this.optionTransferList.push(option); }
  }

  public processTransfer() {
    if (this.optionTransferList && this.optionTransferList.length) {
      this.dataService.postData<OptionTransfer[]>(Modules.Option, this.optionTransferList).then(response => {
        if (response && response.length) { this.optionTransferList = response; } this.optionSubject.next(true);
      });
    }
  }
}
