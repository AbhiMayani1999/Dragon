export const camelCase = (str: string): string => {
  if (str) {
    str = str.replace(/[^a-zA-Z0-9 ]/g, ' ');
    str = str.replace(/([a-z](?=[A-Z]))/g, '$1 ');
    str = str
      .replace(/([^a-zA-Z0-9 ])|^[0-9]+/g, '')
      .trim()
      .toLowerCase();
    str = str.replace(
      /([ 0-9]+)([a-zA-Z])/g,
      (a, b, c) => `${b.trim()}${c.toUpperCase()}`
    );
  }
  return str;
};

export const enumToArray = (data: any): any => {
  const arrayData = [];
  if (data && Object.entries(data).length) {
    for (const [key, value] of Object.entries(data)) {
      if (!Number.isNaN(Number(key))) {
        continue;
      }
      arrayData.push({ key: value, value: key });
    }
  }
  return arrayData;
};

export function newGuid() {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
    return v.toString(16);
  });
}

export function newRandom(length: number): string {
  const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
  return Array.from({ length }, () => characters[Math.floor(Math.random() * characters.length)]).join('');
}
