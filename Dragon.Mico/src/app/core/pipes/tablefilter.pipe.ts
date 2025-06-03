import { Pipe, PipeTransform } from '@angular/core';
import { TablePageData } from 'src/app/dynamics/dycrudtable/dycrudtable.model';

@Pipe({ name: 'tablefilter', standalone: true, pure:false })
export class TablefilterPipe implements PipeTransform {

  transform(data: any[], args: TablePageData): any[] {
    if (!data || !args || !args.isClientSide) { return data; }

    const filterCondition = 'Or';
    let returnData = [];

    // if (args.clearFilter && filtersToApply.length > 0) {
    //   filtersToApply.forEach(singleFilter => {
    //     singleFilter[Object.keys(singleFilter)[0]] = '';
    //   });
    //   args.clearFilter = false;
    // }

    if (args.filter) {
      const filtersToApply = Object.entries(args.filter).map(([key, value]) => ({ key, value })).filter(d => d.value !== null && d.value !== undefined && d.value !== '');
      if (data && data.length && filtersToApply && filtersToApply.length > 0) {
        data.forEach((item) => {
          if (filterCondition === 'Or') {
            let isAdded = false;
            filtersToApply.forEach(singleFilter => {
              if (!isAdded && item[Object.keys(singleFilter)[0]] && item[Object.keys(singleFilter)[0]]
                .toString().toLowerCase()
                .indexOf([Object.values(singleFilter)[0]].toString().toLowerCase()) > -1) { returnData.push(item); isAdded = true; }
            });
          } else {
            let toAddData = true;
            filtersToApply.forEach(singleFilter => {
              toAddData = (toAddData && item[Object.keys(singleFilter)[0]] && item[Object.keys(singleFilter)[0]].toString().toLowerCase()
                .indexOf([Object.values(singleFilter)[0]].toString().toLowerCase()) > -1);
            });
            if (toAddData) { returnData.push(item); }
          }
        });
      } else { returnData = data; }
    }

    // if (returnData && returnData.length && args.orderBy && args.orderBy.key && args.orderBy.value) {
    //   returnData = returnData.sort((a: any, b: any) =>
    //     ((a[args.orderBy.key] && b[args.orderBy.key]) ?
    //       (a[args.orderBy.key] < b[args.orderBy.key]) ? -1 : (a[args.orderBy.key] > b[args.orderBy.key]) ? 1 : 0 :
    //       (!a[args.orderBy.key] && b[args.orderBy.key]) ? -1 : (a[args.orderBy.key] && !b[args.orderBy.key]) ? 1 : 0) * (args.orderBy.value === 'asc' ? 1 : -1));
    // }

    if (data && args.pageSize) {
      if (args.recordsCount > args.pageSize) {
        const pageOffset = (args.currentPage !== 0 ? args.currentPage - 1 : 0) * args.pageSize;
        returnData = returnData.slice(pageOffset, pageOffset + args.pageSize);
      }
    }
    return returnData;
  }
}

// export interface FilterPaginateVal {
//   currentPage: number;
//   filter: any[];
//   isClientSide: boolean;
//   pageSize: number;
//   recordsCount: number;

//   // localSearch: string;
//   // orderBy?: KeyValuePair;
//   // showFilter: boolean;
// }
