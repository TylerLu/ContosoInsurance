export class CommonUtil {

    static getFormatDate(date: Date) {
            return (date != null && date != undefined) ? date.toLocaleDateString() : "";
    }
    static getFormatDateByStr(dateStr: string) {
        return new Date(dateStr).toLocaleDateString();
    }
}